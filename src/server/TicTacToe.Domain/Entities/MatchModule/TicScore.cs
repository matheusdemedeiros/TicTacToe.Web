using TicTacToe.Domain.Entities.BaseModule;

namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicScore : BaseEntity
    {
        public string WinningSymbol { get; private set; }
        public TicPlayer? WinningPlayer { get; private set; }
        public bool Tie { get; private set; }

        public virtual TicMatch Match { get; set; }
        public Guid MatchId { get; set; }

        public TicScore() : base()
        {

        }

        public void Set(TicPlayer winningPlayer)
        {
            if (winningPlayer == null)
            {
                Tie = true;
                WinningSymbol = string.Empty;
                WinningPlayer = null;
            }
            else
            {
                Tie = false;
                WinningSymbol = winningPlayer.Symbol;
                WinningPlayer = winningPlayer;
            }
        }
    }
}