using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Infra.Data.Contexts
{
    public class TicDbContext : DbContext
    {
        //public DbSet<TicMatch> Matches { get; set; }
        public DbSet<TicPlayer> Players { get; set; }

        public TicDbContext(DbContextOptions<TicDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicDbContext).Assembly);
        }
    }
}
