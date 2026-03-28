using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;

namespace TicTacToe.Application.UseCases.Match.AbandonMatch
{
    public record AbandonMatchCommand : IRequest<TicMatchStateResponse>
    {
        public string MatchId { get; set; }
    }
}
