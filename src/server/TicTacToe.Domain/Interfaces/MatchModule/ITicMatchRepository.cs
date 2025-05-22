using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Domain.Interfaces.MatchModule
{
    public interface ITicMatchRepository
    {
        Task<bool> CreateAsync(TicMatch match);
        Task<TicMatch> RetrieveByIdAsync(Guid id);
        Task<List<TicMatch>> RetrieveAllAsync();
        Task<bool> UpdateAsync(TicMatch match);
        Task<bool> DeleteAsync(Guid id);
    }
}
