using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Infra.Data.Contexts;

namespace TicTacToe.Infra.Data
{
    public static class ServiceExtensions
    {
        public static void AddTicPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var ticDbConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TicDbContext>(options =>
                    options.UseSqlServer(ticDbConnectionString));
        }
    }
}
