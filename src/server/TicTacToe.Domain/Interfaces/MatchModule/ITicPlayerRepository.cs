using System.Linq.Expressions;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Domain.Interfaces.MatchModule
{
    public interface ITicPlayerRepository
    {
        Task CreateAsync(TicPlayer player);
        Task<TicPlayer?> RetrieveByIdAsync(Guid id);
        Task<List<TicPlayer>> RetrieveAllAsync();
        Task UpdateAsync(TicPlayer player);
        Task DeleteAsync(Guid id);
        Task<bool> HasAnyWithConditionAsync(Expression<Func<TicPlayer, bool>> condition);
    }
}
