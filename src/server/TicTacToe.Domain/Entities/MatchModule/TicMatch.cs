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
        public virtual TicPlayer? CurrentPlayer { get; private set; }
        public Guid? CurrentPlayerId { get; set; }
        public Guid TicBoardId { get; set; }

        private string _winningSimbol;
        private const int MAX_PLAYERS = 2;

        public TicMatch() : base()
        {
            Players = new List<TicPlayer>();
            Board = new TicBoard();
            State = TicMatchState.NOT_STARTED;
            _winningSimbol = string.Empty;
            TicScore = new TicScore();
            CurrentPlayer = null;
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

            if (State != TicMatchState.NOT_STARTED)
            {
                throw new DomainException("Match has already started or finished. Cannot add more players.");
            }

            SetPlayerSymbol(ticPlayer);

            Players.Add(ticPlayer);
            Touch();
        }


        public void StartMatch()
        {
            if (Players.Count != MAX_PLAYERS)
            {
                throw new DomainException("Match cannot start without two players.");
            }

            if (State != TicMatchState.NOT_STARTED)
            {
                throw new DomainException("Match has already started or finished. Cannot start again.");
            }

            State = TicMatchState.IN_PROGRESS;
            SwitchCurrentPlayer();
            Touch();
        }

        public void FinishMatch()
        {

            if (State != TicMatchState.IN_PROGRESS)
            {
                throw new DomainException("Match is not in progress. Cannot finish.");
            }

            State = TicMatchState.FINISHED;
            SetScore();
            Touch();
        }

        public void MakePlay(string simble, int positionX, int positionY)
        {
            if (State != TicMatchState.IN_PROGRESS)
            {
                throw new DomainException("Match is not in progress. Cannot make a play.");
            }

            Board.MarkCell(simble, positionX, positionY);
            Board.SyncSerializedBoard();
            SwitchCurrentPlayer();
            DetectWin();
            IsTie();
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

        private void SwitchCurrentPlayer()
        {
            if (CurrentPlayer == null)
            {
                CurrentPlayer = Players.FirstOrDefault();
            }
            else
            {
                CurrentPlayer = Players.FirstOrDefault(p => p != CurrentPlayer);
            }
        }

        private void SetPlayerSymbol(TicPlayer ticPlayer)
        {
            ticPlayer.SetSymbol(Players.Count == 0 ? "X" : "O");
        }
    }
}
