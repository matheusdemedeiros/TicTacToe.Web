using Newtonsoft.Json;
using Scalar.AspNetCore;
using TicTacToe.Application.Services;
using TicTacToe.Infra.Data;
using TicTacToe.WebAPI.Extensions;
using TicTacToe.WebAPI.Hubs;
using TicTacToe.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTicPersistence(builder.Configuration);
builder.Services.AddTicApplication();
builder.Services.AddCORSConfig(builder.Configuration);
builder.Services.AddSignalR(e =>
{
    e.EnableDetailedErrors = true;

    e.MaximumReceiveMessageSize = 102400000;
}).AddNewtonsoftJsonProtocol(options =>
{
    options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
    options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseCORSConfig();

app.MapHub<TicMatchHub>("/Ticmatchhub");

app.MapOpenApi();

app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
