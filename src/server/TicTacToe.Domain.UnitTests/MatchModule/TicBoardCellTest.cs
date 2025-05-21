using TicTacToe.Domain.MatchModule;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class TicBoardCellTests
    {
        [Fact]
        public void Constructor_ShouldSetStateToBlank()
        {
            // Arrange & Act
            var cell = new TicBoardCell();

            // Assert
            Assert.Equal(TicBoardCellState.BLANK, cell.State);
            Assert.Null(cell.Symbol);
        }

        [Fact]
        public void MarkCell_WhenStateIsBlank_ShouldSetSymbolAndChangeState()
        {
            // Arrange
            var cell = new TicBoardCell();

            // Act
            cell.MarkCell("X");

            // Assert
            Assert.Equal("X", cell.Symbol);
            Assert.Equal(TicBoardCellState.MARKED, cell.State);
        }

        [Fact]
        public void MarkCell_WhenAlreadyMarked_ShouldNotChangeSymbolOrState()
        {
            // Arrange
            var cell = new TicBoardCell();
            cell.MarkCell("X");

            // Act
            cell.MarkCell("O");

            // Assert
            Assert.Equal("X", cell.Symbol); // não muda
            Assert.Equal(TicBoardCellState.MARKED, cell.State);
        }
    }
}