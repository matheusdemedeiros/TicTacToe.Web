using TicTacToe.Domain.MatchModule;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class TicBoardTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, 2)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 0)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void MarkCell_ShouldMarkCorrectCell(int x, int y)
        {
            // Arrange
            var board = new TicBoard();

            // Act
            board.MarkCell("X", x, y);

            // Assert
            Assert.Equal("X", GetCellSymbol(board, x, y));
        }

        [Fact]
        public void HasWinningSequence_WhenRowIsMarkedWithSameSymbol_ShouldReturnTrue()
        {
            var board = new TicBoard();
            board.MarkCell("X", 0, 0);
            board.MarkCell("X", 0, 1);
            board.MarkCell("X", 0, 2);

            Assert.True(board.HasWinningSequence());
            Assert.Equal("X", board.WinningSimbol);
        }

        [Fact]
        public void HasWinningSequence_WhenColumnIsMarkedWithSameSymbol_ShouldReturnTrue()
        {
            var board = new TicBoard();
            board.MarkCell("O", 0, 0);
            board.MarkCell("O", 1, 0);
            board.MarkCell("O", 2, 0);

            Assert.True(board.HasWinningSequence());
            Assert.Equal("O", board.WinningSimbol);
        }

        [Fact]
        public void HasWinningSequence_WhenPrimaryDiagonalIsMarkedWithSameSymbol_ShouldReturnTrue()
        {
            var board = new TicBoard();
            board.MarkCell("X", 0, 0);
            board.MarkCell("X", 1, 1);
            board.MarkCell("X", 2, 2);

            Assert.True(board.HasWinningSequence());
            Assert.Equal("X", board.WinningSimbol);
        }

        [Fact]
        public void HasWinningSequence_WhenSecondaryDiagonalIsMarkedWithSameSymbol_ShouldReturnTrue()
        {
            var board = new TicBoard();
            board.MarkCell("O", 0, 2);
            board.MarkCell("O", 1, 1);
            board.MarkCell("O", 2, 0);

            Assert.True(board.HasWinningSequence());
            Assert.Equal("O", board.WinningSimbol);
        }

        [Fact]
        public void HasWinningSequence_WhenNoWinningCondition_ShouldReturnFalse()
        {
            var board = new TicBoard();
            board.MarkCell("X", 0, 0);
            board.MarkCell("O", 0, 1);
            board.MarkCell("X", 0, 2);

            Assert.False(board.HasWinningSequence());
            Assert.Null(board.WinningSimbol);
        }

        [Fact]
        public void HasTie_WhenAllCellsFilledAndNoWinner_ShouldReturnTrue()
        {
            var board = new TicBoard();
            var moves = new (int x, int y, string symbol)[]
            {
            (0, 0, "X"), (0, 1, "O"), (0, 2, "X"),
            (1, 0, "X"), (1, 1, "O"), (1, 2, "X"),
            (2, 0, "O"), (2, 1, "X"), (2, 2, "O")
            };

            foreach (var move in moves)
                board.MarkCell(move.symbol, move.x, move.y);

            Assert.True(board.HasTie());
            Assert.Null(board.WinningSimbol);
            Assert.False(board.HasWinningSequence());
        }

        [Fact]
        public void HasTie_WhenBoardNotFull_ShouldReturnFalse()
        {
            var board = new TicBoard();
            board.MarkCell("X", 0, 0);

            Assert.False(board.HasTie());
        }

        [Fact]
        public void HasTie_WhenWinnerExists_ShouldReturnFalse()
        {
            var board = new TicBoard();
            board.MarkCell("X", 0, 0);
            board.MarkCell("X", 0, 1);
            board.MarkCell("X", 0, 2);

            Assert.False(board.HasTie());
            Assert.True(board.HasWinningSequence());
            Assert.Equal("X", board.WinningSimbol);
        }

        // Utilitário de acesso interno (simula um "reflection" leve)
        private string GetCellSymbol(TicBoard board, int x, int y)
        {
            var field = typeof(TicBoard).GetField("_board", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cells = (TicBoardCell[,])field.GetValue(board);
            return cells[x, y].Symbol;
        }
    }
}