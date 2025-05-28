using Scalar.AspNetCore;
using TicTacToe.Application.Services;
using TicTacToe.Infra.Data;
using TicTacToe.WebAPI.Extensions;
using TicTacToe.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTicPersistence(builder.Configuration);
builder.Services.AddTicApplication();
builder.Services.AddCORSConfig(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCORSConfig();

app.MapHub<TicMatchHub>("/Ticmatchhub");

app.MapOpenApi();

app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
