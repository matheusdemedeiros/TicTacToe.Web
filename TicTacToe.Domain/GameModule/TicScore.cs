namespace TicTacToe.Domain.GameModule
{
    public class TicScore
    {
        public string WinningSymbol { get; private set; }
        public TicPlayer? WinningPlayer { get; private set; }
        public bool Tie  { get; private set; }

        public void Set(TicPlayer winningPlayer)
        {
            if(winningPlayer == null)
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