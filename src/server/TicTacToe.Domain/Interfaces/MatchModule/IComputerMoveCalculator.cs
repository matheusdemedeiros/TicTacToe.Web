using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Domain.Interfaces.MatchModule
{
    public interface IComputerMoveCalculator
    {
        (int Row, int Col) Calculate(TicBoard board, string symbol, ComputerDifficulty difficulty);
    }
}
