using Scalar.AspNetCore;
using TicTacToe.Application.Services;
using TicTacToe.Infra.Data;
using TicTacToe.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTicPersistence(builder.Configuration);
builder.Services.AddTicApplication();
builder.Services.ConfigureCORS(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("DefaultPolicy");

app.MapOpenApi();

app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
