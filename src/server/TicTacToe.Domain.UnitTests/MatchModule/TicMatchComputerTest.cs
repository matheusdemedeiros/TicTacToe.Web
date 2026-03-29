using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.UnitTests.MatchModule
{
    public class TicMatchComputerTest
    {
        private TicMatch CreatePvCMatch(ComputerDifficulty difficulty = ComputerDifficulty.Hard)
        {
            return new TicMatch(PlayModeType.PlayerVsComputer, difficulty);
        }

        [Fact]
        public void Constructor_WithPvCAndDifficulty_ShouldSetComputerDifficulty()
        {
            var match = CreatePvCMatch(ComputerDifficulty.Medium);

            Assert.True(match.IsPlayerVsComputer);
            Assert.Equal(ComputerDifficulty.Medium, match.ComputerDifficulty);
        }

        [Fact]
        public void Constructor_WithPvPAndDifficulty_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new TicMatch(PlayModeType.PlayerVsPlayer, ComputerDifficulty.Hard));
        }

        [Fact]
        public void AddComputerPlayer_WhenPvC_ShouldAddCPUPlayer()
        {
            var match = CreatePvCMatch();
            match.AddPlayer(new TicPlayer("Human", "human"));

            var cpu = match.AddComputerPlayer();

            Assert.Equal(2, match.Players.Count);
            Assert.Equal("CPU", cpu.NickName);
            Assert.Equal("Computador", cpu.Name);
        }

        [Fact]
        public void AddComputerPlayer_WhenPvP_ShouldThrowDomainException()
        {
            var match = new TicMatch(PlayModeType.PlayerVsPlayer);

            Assert.Throws<DomainException>(() => match.AddComputerPlayer());
        }

        [Fact]
        public void GetComputerPlayer_ShouldReturnCPUPlayer()
        {
            var match = CreatePvCMatch();
            match.AddPlayer(new TicPlayer("Human", "human"));
            match.AddComputerPlayer();

            var cpu = match.GetComputerPlayer();

            Assert.NotNull(cpu);
            Assert.Equal("CPU", cpu.NickName);
        }

        [Fact]
        public void IsComputerTurn_WhenCPUIsCurrent_ShouldBeTrue()
        {
            var match = CreatePvCMatch();
            match.AddPlayer(new TicPlayer("Human", "human"));
            match.AddComputerPlayer();
            match.StartMatch();

            match.MakePlay("X", 0, 0);

            Assert.True(match.IsComputerTurn);
        }

        [Fact]
        public void MakeComputerPlay_WhenNotComputerTurn_ShouldThrowDomainException()
        {
            var match = CreatePvCMatch();
            match.AddPlayer(new TicPlayer("Human", "human"));
            match.AddComputerPlayer();
            match.StartMatch();

            Assert.Throws<DomainException>(() => match.MakeComputerPlay(1, 1));
        }

        [Fact]
        public void MakeComputerPlay_WhenComputerTurn_ShouldSucceed()
        {
            var match = CreatePvCMatch();
            match.AddPlayer(new TicPlayer("Human", "human"));
            match.AddComputerPlayer();
            match.StartMatch();

            match.MakePlay("X", 0, 0);
            match.MakeComputerPlay(1, 1);

            Assert.Equal("O", match.Board.Board[1][1].Symbol);
            Assert.Equal(TicBoardCellState.MARKED, match.Board.Board[1][1].State);
        }
    }
}
