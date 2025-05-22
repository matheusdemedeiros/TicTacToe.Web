using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicMatch
    {
        public Guid Id { get; private set; }
        public List<TicPlayer> Players { get; private set; }
        public TicBoard Board { get; private set; }
        public TicMatchState State { get; private set; }
        public TicScore TicScore { get; private set; }

        private string _winningSimbol;
        private const int MAX_PLAYERS = 2;

        public TicMatch()
        {
            Id = Guid.NewGuid();
            Players = new List<TicPlayer>();
            Board = new TicBoard();
            State = TicMatchState.NOT_STARTED;
            _winningSimbol = string.Empty;
            TicScore = new TicScore();
        }

        public void AddPlayer(TicPlayer ticPlayer)
        {
            if (Players.Count >= MAX_PLAYERS)
            {
                throw new DomainException("Match already has MAX players.");
            }

            Players.Add(ticPlayer);
        }

        public void StartMatch()
        {
            if (Players.Count != MAX_PLAYERS)
            {
                throw new DomainException("Match cannot start without two players.");
            }

            State = TicMatchState.IN_PROGRESS;
        }

        public void FinishMatch()
        {
            State = TicMatchState.FINISHED;
            SetScore();
        }

        public void MakePlay(string simble, int positionX, int positionY)
        {
            Board.MarkCell(simble, positionX, positionY);
        }

        public void IsTie()
        {
            if (Board.HasTie() && State == TicMatchState.IN_PROGRESS)
            {
                FinishMatch();
            }
        }

        public void DetectWin()
        {
            if (Board.HasWinningSequence() && State == TicMatchState.IN_PROGRESS)
            {
                _winningSimbol = Board.WinningSimbol;
                FinishMatch();
            }
        }

        private void SetScore()
        {
            TicScore.Set(_winningSimbol == string.Empty ? null : Players.FirstOrDefault(p => p.Symbol == _winningSimbol));
        }
    }
}
