namespace TicTacToe.Domain.GameModule
{
    public class TicBoard
    {
        private TicBoardCell[,] _board;
        private const int MAX_WIDTH = 3;
        private const int MAX_HEIGHT = 3;
      

        public TicBoard()
        {
            _board = new TicBoardCell[MAX_WIDTH, MAX_HEIGHT];

            InitializeCells();
        }

        public void MarkCell(string simble, int positionX, int positionY)
        {
            _board[positionX, positionY].MardlCell(simble);
        }

        public void HasWinningSequence()
        {

        }


        private void InitializeCells()
        {
            for (int x = 0; x < MAX_WIDTH; x++)
            {
                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    _board[x, y] = new TicBoardCell();
                }
            }
        }

    }
}
