using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Services.MatchModule;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class ComputerMoveCalculatorTest
    {
        private readonly ComputerMoveCalculator _calculator = new();

        [Fact]
        public void Calculate_Easy_ShouldReturnValidCell()
        {
            var board = new TicBoard(true);

            var (row, col) = _calculator.Calculate(board, "O", ComputerDifficulty.Easy);

            Assert.InRange(row, 0, 2);
            Assert.InRange(col, 0, 2);
            Assert.Equal(TicBoardCellState.BLANK, board.Board[row][col].State);
        }

        [Fact]
        public void Calculate_Hard_ShouldReturnValidCell()
        {
            var board = new TicBoard(true);

            var (row, col) = _calculator.Calculate(board, "O", ComputerDifficulty.Hard);

            Assert.InRange(row, 0, 2);
            Assert.InRange(col, 0, 2);
            Assert.Equal(TicBoardCellState.BLANK, board.Board[row][col].State);
        }

        [Fact]
        public void Calculate_Hard_ShouldBlockOpponentWin()
        {
            var board = new TicBoard(true);
            board.MarkCell("X", 0, 0);
            board.MarkCell("X", 0, 1);

            var (row, col) = _calculator.Calculate(board, "O", ComputerDifficulty.Hard);

            Assert.Equal(0, row);
            Assert.Equal(2, col);
        }

        [Fact]
        public void Calculate_Hard_ShouldWinWhenPossible()
        {
            var board = new TicBoard(true);
            board.MarkCell("O", 0, 0);
            board.MarkCell("O", 0, 1);
            board.MarkCell("X", 1, 0);
            board.MarkCell("X", 1, 1);

            var (row, col) = _calculator.Calculate(board, "O", ComputerDifficulty.Hard);

            Assert.Equal(0, row);
            Assert.Equal(2, col);
        }

        [Fact]
        public void Calculate_Hard_NeverLoses_FullGame()
        {
            var humanMoves = new (int R, int C)[] { (0, 0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2), (2, 0), (2, 1), (2, 2) };

            foreach (var firstMove in humanMoves)
            {
                var board = new TicBoard(true);
                board.MarkCell("X", firstMove.R, firstMove.C);

                while (!board.HasWinningSequence() && !board.HasTie())
                {
                    var (cr, cc) = _calculator.Calculate(board, "O", ComputerDifficulty.Hard);
                    board.MarkCell("O", cr, cc);

                    if (board.HasWinningSequence() || board.HasTie()) break;

                    var blanks = GetBlanks(board);
                    if (blanks.Count == 0) break;
                    var pick = blanks[Random.Shared.Next(blanks.Count)];
                    board.MarkCell("X", pick.R, pick.C);
                }

                if (board.HasWinningSequence())
                {
                    Assert.NotEqual("X", board.WinningSimbol);
                }
            }
        }

        [Fact]
        public void Calculate_Medium_ShouldReturnValidCell()
        {
            var board = new TicBoard(true);

            var (row, col) = _calculator.Calculate(board, "O", ComputerDifficulty.Medium);

            Assert.InRange(row, 0, 2);
            Assert.InRange(col, 0, 2);
            Assert.Equal(TicBoardCellState.BLANK, board.Board[row][col].State);
        }

        private static List<(int R, int C)> GetBlanks(TicBoard board)
        {
            var blanks = new List<(int, int)>();
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board.Board[r][c].State == TicBoardCellState.BLANK)
                        blanks.Add((r, c));
            return blanks;
        }
    }
}
