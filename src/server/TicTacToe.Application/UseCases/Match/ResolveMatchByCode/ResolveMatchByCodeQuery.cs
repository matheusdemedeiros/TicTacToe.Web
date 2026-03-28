using MediatR;

namespace TicTacToe.Application.UseCases.Match.ResolveMatchByCode
{
    public record ResolveMatchByCodeQuery : IRequest<ResolveMatchByCodeResponse>
    {
        public string ShortCode { get; set; }
    }
}
