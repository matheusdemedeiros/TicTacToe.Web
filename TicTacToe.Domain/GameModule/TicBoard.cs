namespace TicTacToe.Domain.GameModule
{
    public class TicBoard
    {
        public string WinningSimbol { get; private set; }

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
            _board[positionX, positionY].MarkCell(simble);
        }

        public bool HasWinningSequence()
        {
            return CheckRowForWin() != null ||
                   CheckColumnForWin() != null ||
                   CheckDiagonalsForWin() != null;
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

        private List<TicBoardCell> CheckRowForWin()
        {
            for (int row = 0; row < MAX_HEIGHT; row++)
            {
                if (_board[row, 0].State != TicBoardCellState.BLANK &&
                    _board[row, 0].Simble == _board[row, 1].Simble &&
                    _board[row, 1].Simble == _board[row, 2].Simble)
                {
                    WinningSimbol = _board[row, 0].Simble;
                    return new List<TicBoardCell>
                            {
                                _board[row, 0],
                                _board[row, 1],
                                _board[row, 2]
                            };
                }
            }

            return null;
        }

        private List<TicBoardCell> CheckColumnForWin()
        {
            for (int column = 0; column < MAX_WIDTH; column++)
            {
                if (_board[0, column].State != TicBoardCellState.BLANK &&
                    _board[0, column].Simble == _board[1, column].Simble &&
                    _board[1, column].Simble == _board[2, column].Simble)
                {
                    WinningSimbol = _board[0, column].Simble;
                    return new List<TicBoardCell>
                        {
                            _board[0, column],
                            _board[1, column],
                            _board[2, column]
                        };
                }
            }

            return null;
        }

        private List<TicBoardCell> CheckDiagonalsForWin()
        {
            if (_board[0, 0].State != TicBoardCellState.BLANK &&
                _board[0, 0].Simble == _board[1, 1].Simble &&
                _board[1, 1].Simble == _board[2, 2].Simble)
            {
                WinningSimbol = _board[0, 0].Simble;
                return new List<TicBoardCell>
                        {
                            _board[0, 0],
                            _board[1, 1],
                            _board[2, 2]
                        };
            }

            if (_board[0, 2].State != TicBoardCellState.BLANK &&
                _board[0, 2].Simble == _board[1, 1].Simble &&
                _board[1, 1].Simble == _board[2, 0].Simble)
            {
                WinningSimbol = _board[0, 2].Simble;
                return new List<TicBoardCell>
                        {
                            _board[0, 2],
                            _board[1, 1],
                            _board[2, 0]
                        };
            }

            return null;
        }
    }
}
