using System.Linq.Expressions;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Domain.Interfaces.MatchModule
{
    public interface ITicMatchRepository
    {
        Task CreateAsync(TicMatch match);
        Task<TicMatch?> RetrieveByIdAsync(Guid id);
        Task<List<TicMatch>> RetrieveAllAsync();
        Task UpdateAsync(TicMatch match);
        Task DeleteAsync(Guid id);
        Task<bool> HasAnyWithConditionAsync(Expression<Func<TicMatch, bool>> condition);
    }
}
