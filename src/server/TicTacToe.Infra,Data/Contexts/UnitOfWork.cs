using TicTacToe.Domain.Interfaces;
using TicTacToe.Infra.Data.Contexts;

namespace TicTacToe.Infra_Data.Contexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicDbContext _context;
        public UnitOfWork(TicDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
