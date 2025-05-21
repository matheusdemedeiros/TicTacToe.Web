using TicTacToe.Domain.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class TicMatchTest
    {
        private TicPlayer CreatePlayer(string name, string symbol) =>
            new TicPlayer(name, $"{name.ToLower()}123", symbol);

        [Fact]
        public void AddPlayer_ShouldAddPlayersUntilMax()
        {
            var match = new TicMatch();
            var p1 = CreatePlayer("Alice", "X");
            var p2 = CreatePlayer("Bob", "O");

            match.AddPlayer(p1);
            match.AddPlayer(p2);

            Assert.Equal(2, match.Players.Count);
        }

        [Fact]
        public void AddPlayer_WhenMatchHasMaxPlayers_ShouldThrowDomainException()
        {
            var match = new TicMatch();
            var p1 = CreatePlayer("Alice", "X");
            var p2 = CreatePlayer("Bob", "O");
            var p3 = CreatePlayer("Charlie", "Z");

            match.AddPlayer(p1);
            match.AddPlayer(p2);

            var ex = Assert.Throws<DomainException>(() => match.AddPlayer(p3));
            Assert.Equal("Match already has MAX players.", ex.Message);
        }

        [Fact]
        public void StartMatch_WithTwoPlayers_SetsStateToInProgress()
        {
            var match = new TicMatch();
            match.AddPlayer(CreatePlayer("Alice", "X"));
            match.AddPlayer(CreatePlayer("Bob", "O"));

            match.StartMatch();

            Assert.Equal(TicMatchState.IN_PROGRESS, match.State);
        }

        [Fact]
        public void StartMatch_WithoutTwoPlayers_ThrowsException()
        {
            var match = new TicMatch();
            match.AddPlayer(CreatePlayer("Solo", "X"));

            Assert.Throws<DomainException>(() => match.StartMatch());

            var ex = Assert.Throws<DomainException>(() => match.StartMatch());
            Assert.Equal("Match cannot start without two players.", ex.Message);
        }

        [Fact]
        public void MakePlay_UpdatesBoardState()
        {
            var match = new TicMatch();
            match.AddPlayer(CreatePlayer("Alice", "X"));
            match.AddPlayer(CreatePlayer("Bob", "O"));
            match.StartMatch();

            match.MakePlay("X", 0, 0);

            // Acesso ao campo privado _board via reflexão
            var boardField = typeof(TicBoard).GetField("_board", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var board = (TicBoardCell[,])boardField.GetValue(match.Board);

            Assert.Equal("X", board[0, 0].Symbol);
        }

        [Fact]
        public void DetectWin_WhenWinningSequenceExists_SetsMatchAsFinished()
        {
            var match = new TicMatch();
            var p1 = CreatePlayer("Alice", "X");
            var p2 = CreatePlayer("Bob", "O");

            match.AddPlayer(p1);
            match.AddPlayer(p2);
            match.StartMatch();

            match.MakePlay("X", 0, 0);
            match.MakePlay("X", 0, 1);
            match.MakePlay("X", 0, 2);

            match.DetectWin();

            Assert.Equal(TicMatchState.FINISHED, match.State);
            Assert.Equal(p1, match.TicScore.WinningPlayer);
        }

        [Fact]
        public void IsTie_WhenBoardFullWithoutWinner_SetsMatchAsTied()
        {
            var match = new TicMatch();
            match.AddPlayer(CreatePlayer("Alice", "X"));
            match.AddPlayer(CreatePlayer("Bob", "O"));
            match.StartMatch();

            var plays = new (int x, int y, string symbol)[]
            {
            (0,0,"X"), (0,1,"O"), (0,2,"X"),
            (1,0,"X"), (1,1,"O"), (1,2,"X"),
            (2,0,"O"), (2,1,"X"), (2,2,"O")
            };

            foreach (var (x, y, s) in plays)
                match.MakePlay(s, x, y);

            match.IsTie();

            Assert.True(match.TicScore.Tie);
            Assert.Equal(TicMatchState.FINISHED, match.State);
        }
    }
}