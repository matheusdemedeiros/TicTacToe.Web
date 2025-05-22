namespace TicTacToe.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
