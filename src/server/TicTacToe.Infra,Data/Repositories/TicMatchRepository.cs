using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Infra.Data.Contexts;

namespace TicTacToe.Infra_Data.Repositories
{
    public class TicMatchRepository : ITicMatchRepository
    {
        private readonly TicDbContext _context;
        private readonly DbSet<TicMatch> _matches;

        public TicMatchRepository(TicDbContext context)
        {
            _context = context;
            _matches = context.Matches;
        }

        public async Task CreateAsync(TicMatch match)
        {
            await _matches.AddAsync(match);
        }

        public Task UpdateAsync(TicMatch match)
        {
            _matches.Update(match);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _matches.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<TicMatch>> RetrieveAllAsync()
        {
            return await _matches.AsNoTracking().ToListAsync();
        }

        public async Task<TicMatch?> RetrieveByIdAsync(Guid id)
        {
            return await _matches.FindAsync(id);
        }

        public async Task<bool> HasAnyWithConditionAsync(Expression<Func<TicMatch, bool>> condition)
        {
            return await _matches.AnyAsync(condition);
        }
    }
}
