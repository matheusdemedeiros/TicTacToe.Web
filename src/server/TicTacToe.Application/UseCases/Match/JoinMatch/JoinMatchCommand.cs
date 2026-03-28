using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;

namespace TicTacToe.Application.UseCases.Match.JoinMatch
{
    public record JoinMatchCommand : IRequest<TicMatchStateResponse>
    {
        public string MatchId { get; set; }
    }
}
