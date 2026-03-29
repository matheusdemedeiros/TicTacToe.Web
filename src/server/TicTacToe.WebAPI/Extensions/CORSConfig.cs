namespace TicTacToe.WebAPI.Extensions
{
    public static class CORSConfig
    {
        public static IServiceCollection AddCORSConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                     policy =>
                     {
                         policy.WithOrigins(allowedOrigins)
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
