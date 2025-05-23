using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Infra.Data.Contexts;

namespace TicTacToe.Infra.Data.Repositories
{
    public class TicPlayerRepository : ITicPlayerRepository
    {
        private readonly TicDbContext _context;
        private readonly DbSet<TicPlayer> _players;

        public TicPlayerRepository(TicDbContext context)
        {
            _context = context;
            _players = context.Players;
        }

        public async Task CreateAsync(TicPlayer player)
        {
            await _players.AddAsync(player);
        }

        public Task UpdateAsync(TicPlayer player)
        {
            _players.Update(player);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _players.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<TicPlayer>> RetrieveAllAsync()
        {
            return await _players.AsNoTracking().ToListAsync();
        }

        public async Task<TicPlayer?> RetrieveByIdAsync(Guid id)
        {
            return await _players.FindAsync(id);
        }

        public async Task<bool> HasAnyWithConditionAsync(Expression<Func<TicPlayer, bool>> condition)
        {
            return await _players.AnyAsync(condition);
        }
    }
}
