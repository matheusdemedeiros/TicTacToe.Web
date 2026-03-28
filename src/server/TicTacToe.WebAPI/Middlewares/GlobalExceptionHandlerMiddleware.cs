using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TicTacToe.Domain.SharedModule.Exceptions;
using TicTacToe.WebAPI.Models;

namespace TicTacToe.WebAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain exception: {Message}", ex.Message);
                await WriteErrorResponseAsync(context, HttpStatusCode.BadRequest, ex.Message, "DOMAIN_ERROR");
            }
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Format exception: {Message}", ex.Message);
                await WriteErrorResponseAsync(context, HttpStatusCode.BadRequest, "Invalid input format.", "VALIDATION_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await WriteErrorResponseAsync(context, HttpStatusCode.InternalServerError, ex.Message, "INTERNAL_ERROR");
            }
        }

        private static async Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, string message, string errorCode)
        {
            var response = new ApiErrorResponse(message, errorCode, (int)statusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonConvert.SerializeObject(response, JsonSettings);
            await context.Response.WriteAsync(json);
        }
    }
}
