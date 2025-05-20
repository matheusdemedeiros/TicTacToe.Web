namespace TicTacToe.Domain.GameModule
{
    public class TicBoardCell
    {
        public string Simble { get; private set; }
        public TicBoardCellState State { get; private set; }

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

            Simble = simble;
            State = TicBoardCellState.MARKED;
        }
    }

    public enum TicBoardCellState
    {
        BLANK = 0,
        MARKED = 1
    }

}
