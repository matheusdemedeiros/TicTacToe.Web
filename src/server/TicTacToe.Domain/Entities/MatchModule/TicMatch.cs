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
        public ComputerDifficulty? ComputerDifficulty { get; private set; }
        public virtual TicPlayer? CurrentPlayer { get; private set; }
        public Guid? CurrentPlayerId { get; set; }
        public Guid TicBoardId { get; set; }
        public string ShortCode { get; private set; }

        private string _winningSimbol;
        private const int MAX_PLAYERS = 2;
        private const int SHORT_CODE_LENGTH = 6;
        private const string SHORT_CODE_CHARS = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private const string COMPUTER_PLAYER_NAME = "Computador";
        private const string COMPUTER_PLAYER_NICKNAME = "CPU";

        public TicMatch() : base()
        {
            Players = new List<TicPlayer>();
            Board = new TicBoard();
            State = TicMatchState.NOT_STARTED;
            _winningSimbol = string.Empty;
            TicScore = new TicScore();
            CurrentPlayer = null;
            ShortCode = GenerateShortCode();
        }

        public TicMatch(PlayModeType playMode) : this()
        {
            PlayMode = playMode;
        }

        public TicMatch(PlayModeType playMode, ComputerDifficulty difficulty) : this()
        {
            if (playMode != PlayModeType.PlayerVsComputer)
                throw new DomainException("Computer difficulty can only be set for Player vs Computer matches.");

            PlayMode = playMode;
            ComputerDifficulty = difficulty;
        }

        public bool IsPlayerVsComputer => PlayMode == PlayModeType.PlayerVsComputer;

        public bool IsComputerTurn => IsPlayerVsComputer
            && State == TicMatchState.IN_PROGRESS
            && CurrentPlayer != null
            && CurrentPlayer.NickName == COMPUTER_PLAYER_NICKNAME;

        public TicPlayer? GetComputerPlayer()
        {
            return Players.FirstOrDefault(p => p.NickName == COMPUTER_PLAYER_NICKNAME);
        }

        public TicPlayer AddComputerPlayer()
        {
            if (!IsPlayerVsComputer)
                throw new DomainException("Cannot add computer player to a Player vs Player match.");

            var computerPlayer = new TicPlayer(COMPUTER_PLAYER_NAME, COMPUTER_PLAYER_NICKNAME);
            AddPlayer(computerPlayer);
            return computerPlayer;
        }

        public void MakeComputerPlay(int positionX, int positionY)
        {
            if (!IsComputerTurn)
                throw new DomainException("It is not the computer's turn.");

            MakePlay(CurrentPlayer!.Symbol, positionX, positionY);
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

        public void Abandon()
        {
            if (State == TicMatchState.FINISHED)
            {
                throw new DomainException("Match already finished.");
            }

            State = TicMatchState.FINISHED;
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
            DetectWin();
            IsTie();

            if (State == TicMatchState.IN_PROGRESS)
            {
                SwitchCurrentPlayer();
            }

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

        private static string GenerateShortCode()
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[SHORT_CODE_LENGTH];
            rng.GetBytes(bytes);

            var chars = new char[SHORT_CODE_LENGTH];
            for (int i = 0; i < SHORT_CODE_LENGTH; i++)
            {
                chars[i] = SHORT_CODE_CHARS[bytes[i] % SHORT_CODE_CHARS.Length];
            }

            return new string(chars);
        }
    }
}
