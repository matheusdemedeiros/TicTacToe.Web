using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Domain.Services.MatchModule
{
    public class ComputerMoveCalculator : IComputerMoveCalculator
    {
        private const int BoardSize = 3;
        private const int MaxScore = 10;
        private const int MinScore = -10;
        private const int TieScore = 0;

        public (int Row, int Col) Calculate(TicBoard board, string symbol, ComputerDifficulty difficulty)
        {
            var availableCells = GetAvailableCells(board);

            if (availableCells.Count == 0)
                throw new DomainException("No available cells to play.");

            return difficulty switch
            {
                ComputerDifficulty.Easy => PickRandomMove(availableCells),
                ComputerDifficulty.Medium => PickMediumMove(board, symbol, availableCells),
                ComputerDifficulty.Hard => PickBestMove(board, symbol),
                _ => throw new DomainException("Invalid computer difficulty.")
            };
        }

        private static (int Row, int Col) PickRandomMove(List<(int Row, int Col)> availableCells)
        {
            var index = Random.Shared.Next(availableCells.Count);
            return availableCells[index];
        }

        private (int Row, int Col) PickMediumMove(TicBoard board, string symbol, List<(int Row, int Col)> availableCells)
        {
            var useOptimal = Random.Shared.Next(100) < 50;
            return useOptimal ? PickBestMove(board, symbol) : PickRandomMove(availableCells);
        }

        private (int Row, int Col) PickBestMove(TicBoard board, string symbol)
        {
            var opponentSymbol = GetOpponentSymbol(symbol);
            var bestScore = int.MinValue;
            (int Row, int Col) bestMove = (-1, -1);

            var cells = board.Board;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (cells[row][col].State != TicBoardCellState.BLANK)
                        continue;

                    cells[row][col].MarkCell(symbol);
                    var score = Minimax(cells, opponentSymbol, symbol, isMaximizing: false, depth: 0);
                    cells[row][col].Reset();

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }

            return bestMove;
        }

        private int Minimax(TicBoardCell[][] cells, string currentSymbol, string computerSymbol, bool isMaximizing, int depth)
        {
            var opponentSymbol = GetOpponentSymbol(computerSymbol);

            if (HasWinner(cells, computerSymbol))
                return MaxScore - depth;

            if (HasWinner(cells, opponentSymbol))
                return MinScore + depth;

            if (!HasBlankCells(cells))
                return TieScore;

            var bestScore = isMaximizing ? int.MinValue : int.MaxValue;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (cells[row][col].State != TicBoardCellState.BLANK)
                        continue;

                    cells[row][col].MarkCell(currentSymbol);
                    var nextSymbol = currentSymbol == computerSymbol ? opponentSymbol : computerSymbol;
                    var score = Minimax(cells, nextSymbol, computerSymbol, !isMaximizing, depth + 1);
                    cells[row][col].Reset();

                    bestScore = isMaximizing
                        ? Math.Max(bestScore, score)
                        : Math.Min(bestScore, score);
                }
            }

            return bestScore;
        }

        private static List<(int Row, int Col)> GetAvailableCells(TicBoard board)
        {
            var cells = new List<(int, int)>();
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board.Board[row][col].State == TicBoardCellState.BLANK)
                        cells.Add((row, col));
                }
            }
            return cells;
        }

        private static bool HasWinner(TicBoardCell[][] cells, string symbol)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (cells[i][0].Symbol == symbol && cells[i][1].Symbol == symbol && cells[i][2].Symbol == symbol)
                    return true;
                if (cells[0][i].Symbol == symbol && cells[1][i].Symbol == symbol && cells[2][i].Symbol == symbol)
                    return true;
            }

            if (cells[0][0].Symbol == symbol && cells[1][1].Symbol == symbol && cells[2][2].Symbol == symbol)
                return true;
            if (cells[0][2].Symbol == symbol && cells[1][1].Symbol == symbol && cells[2][0].Symbol == symbol)
                return true;

            return false;
        }

        private static bool HasBlankCells(TicBoardCell[][] cells)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (cells[row][col].State == TicBoardCellState.BLANK)
                        return true;
                }
            }
            return false;
        }

        private static string GetOpponentSymbol(string symbol)
        {
            return symbol == "X" ? "O" : "X";
        }
    }
}
