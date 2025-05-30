using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class TicPlayerTest
    {
        [Fact]
        public void Constructor_WithValidNameAndNickName_ShouldSetProperties()
        {
            // Arrange
            var name = "João";
            var nickName = "joaozin";

            // Act
            var player = new TicPlayer(name, nickName);

            // Assert
            Assert.Equal(name, player.Name);
            Assert.Equal(nickName, player.NickName);
        }

        [Theory]
        [InlineData(null, "nick")]
        [InlineData("", "nick")]
        [InlineData("name", null)]
        [InlineData("name", "")]
        [InlineData("", "")]
        public void Constructor_WithNullOrEmptyNameOrNickName_ShouldThrowDomainException(string name, string nickName)
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new TicPlayer(name, nickName));
            Assert.Equal("Name and NickName cannot be null or empty.", ex.Message);
        }

        [Theory]
        [InlineData("valid", "valid", null)]
        [InlineData("valid", "valid", "")]
        public void Constructor_WithNullOrEmptySymbol_ShouldThrowDomainException(string name, string nickName, string symbol)
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new TicPlayer(name, nickName));
            Assert.Equal("TicPlayer symbol is required.", ex.Message);
        }
    }
}

