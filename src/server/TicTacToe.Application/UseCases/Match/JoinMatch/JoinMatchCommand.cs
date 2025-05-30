using MediatR;

namespace TicTacToe.Application.UseCases.Match.JoinMatch
{
    public record JoinMatchCommand : IRequest<JoinMatchResponse>
    {
        public string MatchId { get; set; }
    }
}
