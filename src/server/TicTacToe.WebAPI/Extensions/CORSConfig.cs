namespace TicTacToe.WebAPI.Extensions
{
    public static class CORSConfig
    {
        public static IServiceCollection AddCORSConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                     policy =>
                     {
                         policy.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                     });
            });

            return services;
        }

        public static IApplicationBuilder UseCORSConfig(this IApplicationBuilder app)
        {
            app.UseCors("DefaultPolicy");

            return app;
        }
    }
}
