using MediatR;

namespace TicTacToe.Application.UseCases.Match.AddPlayer
{
    public record AddTicPlayerToMatchCommand : IRequest<AddTicPlayerPlayerResponse>
    {
        public string PlayerId { get; set; }
        public string MatchId { get; set; }
    }
}
