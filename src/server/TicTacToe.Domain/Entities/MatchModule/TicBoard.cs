namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicBoard
    {
        public string WinningSimbol { get; private set; }

        private TicBoardCell[,] _board;
        private const int MAX_WIDTH = 3;
        private const int MAX_HEIGHT = 3;

        public TicBoardCell[,] Board { get => _board; }

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

        public bool HasTie()
        {
            return !HasBlankCells() && !HasWinningSequence();
        }

        private bool HasBlankCells()
        {
            for (int x = 0; x < MAX_WIDTH; x++)
            {
                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    if (_board[x, y].State == TicBoardCellState.BLANK)
                    {
                        return true;
                    }
                }
            }

            return false;
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
                    _board[row, 0].Symbol == _board[row, 1].Symbol &&
                    _board[row, 1].Symbol == _board[row, 2].Symbol)
                {
                    WinningSimbol = _board[row, 0].Symbol;
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
                    _board[0, column].Symbol == _board[1, column].Symbol &&
                    _board[1, column].Symbol == _board[2, column].Symbol)
                {
                    WinningSimbol = _board[0, column].Symbol;
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
                _board[0, 0].Symbol == _board[1, 1].Symbol &&
                _board[1, 1].Symbol == _board[2, 2].Symbol)
            {
                WinningSimbol = _board[0, 0].Symbol;
                return new List<TicBoardCell>
                        {
                            _board[0, 0],
                            _board[1, 1],
                            _board[2, 2]
                        };
            }

            if (_board[0, 2].State != TicBoardCellState.BLANK &&
                _board[0, 2].Symbol == _board[1, 1].Symbol &&
                _board[1, 1].Symbol == _board[2, 0].Symbol)
            {
                WinningSimbol = _board[0, 2].Symbol;
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
