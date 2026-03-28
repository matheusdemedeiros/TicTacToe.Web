using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;

namespace TicTacToe.Application.UseCases.Match.Rematch
{
    public record RematchCommand : IRequest<TicMatchStateResponse>
    {
        public string PreviousMatchId { get; set; }
    }
}
