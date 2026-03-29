namespace TicTacToe.WebAPI.Models
{
    public record ApiErrorResponse
    {
        public string Message { get; init; }
        public string ErrorCode { get; init; }
        public int StatusCode { get; init; }

        public ApiErrorResponse(string message, string errorCode, int statusCode)
        {
            Message = message;
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
