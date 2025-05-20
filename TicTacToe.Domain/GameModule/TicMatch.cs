namespace TicTacToe.Domain.GameModule
{
    public class TicMatch
    {
        public Guid Id { get; private set; }
        public List<TicPlayer> Players { get; private set; }
        public TicBoard Board { get; private set; }
        public TicMatchState State { get; private set; }
        public string WinningSimbol { get; private set; }

        private const int MAX_PLAYERS = 2;

        public TicMatch()
        {
            Id = Guid.NewGuid();
            Players = new List<TicPlayer>();
            Board = new TicBoard();
            State = TicMatchState.NOT_STARTED;
            WinningSimbol = string.Empty;
        }

        public void AddPlayer(TicPlayer ticPlayer)
        {
            if (Players.Count >= MAX_PLAYERS)
            {
                return;
            }

            Players.Add(ticPlayer);
        }

        public void StartMatch()
        {
            if (Players.Count != MAX_PLAYERS)
            {
                throw new InvalidOperationException("Match cannot start without two players.");
            }

            State = TicMatchState.IN_PROGRESS;
        }

        public void FinishMatch()
        {
            State = TicMatchState.FINISHED;
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
                WinningSimbol = Board.WinningSimbol;
                FinishMatch();
            }
        }
    }
}
