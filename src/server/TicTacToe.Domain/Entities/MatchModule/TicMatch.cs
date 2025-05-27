using TicTacToe.Domain.Entities.BaseModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicMatch : BaseEntity
    {
        public List<TicPlayer> Players { get; private set; }
        public virtual TicBoard Board { get; private set; }
        public TicMatchState State { get; private set; }
        public virtual TicScore TicScore { get; private set; }
        public PlayModeType PlayMode { get; private set; }

        private string _winningSimbol;
        private const int MAX_PLAYERS = 2;

        public TicMatch() : base()
        {
            Players = new List<TicPlayer>();
            Board = new TicBoard();
            State = TicMatchState.NOT_STARTED;
            _winningSimbol = string.Empty;
            TicScore = new TicScore();
        }

        public TicMatch(PlayModeType playMode) : this()
        {
            PlayMode = playMode;
        }

        public void AddPlayer(TicPlayer ticPlayer)
        {
            if (Players.Count >= MAX_PLAYERS)
            {
                throw new DomainException("Match already has MAX players.");
            }

            Players.Add(ticPlayer);
            Touch();
        }

        public void StartMatch()
        {
            if (Players.Count != MAX_PLAYERS)
            {
                throw new DomainException("Match cannot start without two players.");
            }

            State = TicMatchState.IN_PROGRESS;
            Touch();
        }

        public void FinishMatch()
        {
            State = TicMatchState.FINISHED;
            SetScore();
            Touch();
        }

        public void MakePlay(string simble, int positionX, int positionY)
        {
            Board.MarkCell(simble, positionX, positionY);
            Touch();
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
