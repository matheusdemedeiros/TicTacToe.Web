using TicTacToe.Domain.MatchModule;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class TicScoreTest
    {
        [Fact]
        public void Set_WithWinningPlayer_ShouldSetWinnerAndSymbolAndTieFalse()
        {
            // Arrange
            var player = new TicPlayer("João", "joaozin") { Symbol = "X" };
            var score = new TicScore();

            // Act
            score.Set(player);

            // Assert
            Assert.False(score.Tie);
            Assert.Equal("X", score.WinningSymbol);
            Assert.Equal(player, score.WinningPlayer);
        }

        [Fact]
        public void Set_WithNullWinningPlayer_ShouldSetTieTrueAndClearWinner()
        {
            // Arrange
            var score = new TicScore();

            // Act
            score.Set(null);

            // Assert
            Assert.True(score.Tie);
            Assert.Equal(string.Empty, score.WinningSymbol);
            Assert.Null(score.WinningPlayer);
        }
    }
}