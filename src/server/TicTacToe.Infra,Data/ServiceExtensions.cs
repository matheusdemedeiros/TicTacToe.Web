using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Infra.Data.Contexts;
using TicTacToe.Infra.Data.Repositories;
using TicTacToe.Infra_Data.Contexts;
using TicTacToe.Infra_Data.Repositories;

namespace TicTacToe.Infra.Data
{
    public static class ServiceExtensions
    {
        public static void AddTicPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var ticDbConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TicDbContext>(options =>
                    options.UseSqlServer(ticDbConnectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITicPlayerRepository, TicPlayerRepository>();
            services.AddScoped<ITicMatchRepository, TicMatchRepository>();
        }
    }
}
