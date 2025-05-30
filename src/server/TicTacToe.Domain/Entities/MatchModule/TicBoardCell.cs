namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicBoardCell
    {
        public string Symbol { get; set; }
        public TicBoardCellState State { get; set; }

        public TicBoardCell()
        {
            State = TicBoardCellState.BLANK;
        }

        public void MarkCell(string simble)
        {
            if (State != TicBoardCellState.BLANK)
            {
                return;
            }

            Symbol = simble;
            State = TicBoardCellState.MARKED;
        }
    }

    public enum TicBoardCellState
    {
        BLANK = 0,
        MARKED = 1
    }

}
