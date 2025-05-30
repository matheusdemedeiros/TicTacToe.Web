using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using TicTacToe.Domain.Entities.BaseModule;

namespace TicTacToe.Domain.Entities.MatchModule
{
    public class TicBoard : BaseEntity
    {
        public string? WinningSimbol { get; private set; }

        private TicBoardCell[][] _board;
        private const int MAX_WIDTH = 3;
        private const int MAX_HEIGHT = 3;

        public TicBoard() : base()
        {
            // O _board não é inicializado aqui para permitir que o EF Core popule SerializedBoard primeiro.
            // A inicialização/desserialização ocorrerá no getter da propriedade Board.
        }

        // Construtor para criar um novo tabuleiro vazio
        public TicBoard(bool initializeEmpty = true) : base()
        {
            if (initializeEmpty)
            {
                _board = CreateEmptyBoard();
                SyncSerializedBoard(); // Sincroniza o board vazio na criação
            }
        }


        [NotMapped]
        public TicBoardCell[][] Board
        {
            get
            {
                // Carrega ou inicializa o _board na primeira vez que é acessado após o carregamento ou criação.
                if (_board == null)
                {
                    if (!string.IsNullOrEmpty(SerializedBoard))
                    {
                        // Tenta desserializar do banco se houver dados
                        _board = JsonConvert.DeserializeObject<TicBoardCell[][]>(SerializedBoard);
                        // Verifica se a desserialização resultou em null (pode acontecer com dados corrompidos)
                        if (_board == null)
                        {
                            _board = CreateEmptyBoard(); // Fallback para tabuleiro vazio
                        }
                    }
                    else
                    {
                        // Se não houver SerializedBoard, cria um tabuleiro vazio
                        _board = CreateEmptyBoard();
                    }
                }
                return _board;
            }
            // O setter privado só permite que a própria classe defina _board
            private set
            {
                _board = value;
                SyncSerializedBoard(); // Sempre sincroniza quando o _board é setado diretamente (ex: na criação)
            }
        }

        // Persistido no banco
        public string SerializedBoard { get; private set; } = string.Empty; // Inicialize para evitar null warnings

        // Este método deve ser chamado sempre que o _board for modificado
        public void SyncSerializedBoard()
        {
            // Apenas serializa se _board já foi carregado/inicializado
            if (_board != null)
            {
                SerializedBoard = JsonConvert.SerializeObject(_board);
            }
        }

        public void MarkCell(string simble, int positionX, int positionY)
        {
            Board[positionX][positionY].MarkCell(simble);
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
                    if (Board[x][y].State == TicBoardCellState.BLANK)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<TicBoardCell> CheckRowForWin()
        {
            for (int row = 0; row < MAX_HEIGHT; row++)
            {
                if (Board[row][0].State != TicBoardCellState.BLANK &&
                    Board[row][0].Symbol == Board[row][1].Symbol &&
                    Board[row][1].Symbol == Board[row][2].Symbol)
                {
                    WinningSimbol = _board[row][0].Symbol;
                    return new List<TicBoardCell>
                            {
                                Board[row][0],
                                Board[row][1],
                                Board[row][2]
                            };
                }
            }

            return null;
        }

        private List<TicBoardCell> CheckColumnForWin()
        {
            for (int column = 0; column < MAX_WIDTH; column++)
            {
                if (Board[0][column].State != TicBoardCellState.BLANK &&
                    Board[0][column].Symbol == Board[1][column].Symbol &&
                    Board[1][column].Symbol == Board[2][column].Symbol)
                {
                    WinningSimbol = _board[0][column].Symbol;
                    return new List<TicBoardCell>
                        {
                            _board[0][column],
                            _board[1][column],
                            _board[2][column]
                        };
                }
            }

            return null;
        }

        private List<TicBoardCell> CheckDiagonalsForWin()
        {
            if (Board[0][0].State != TicBoardCellState.BLANK &&
                Board[0][0].Symbol == Board[1][1].Symbol &&
                Board[1][1].Symbol == Board[2][2].Symbol)
            {
                WinningSimbol = Board[0][0].Symbol;
                return new List<TicBoardCell>
                        {
                            Board[0][0],
                            Board[1][1],
                            Board[2][2]
                        };
            }

            if (Board[0][2].State != TicBoardCellState.BLANK &&
                Board[0][2].Symbol == Board[1][1].Symbol &&
                Board[1][1].Symbol == Board[2][0].Symbol)
            {
                WinningSimbol = Board[0][2].Symbol;
                return new List<TicBoardCell>
                        {
                            Board[0][2],
                            Board[1][1],
                            Board[2][0]
                        };
            }

            return null;
        }

        private TicBoardCell[][] CreateEmptyBoard()
        {
            var board = new TicBoardCell[MAX_WIDTH][];
            for (int x = 0; x < MAX_WIDTH; x++)
            {
                board[x] = new TicBoardCell[MAX_HEIGHT];
                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    board[x][y] = new TicBoardCell();
                }
            }
            return board;
        }
    }
}
