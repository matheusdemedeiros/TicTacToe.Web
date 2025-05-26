using MediatR;
using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Application.UseCases.Match.CreateMatch
{
    public record CreateTicMatchCommand : IRequest<CreateTicMatchResponse>
    {
        public PlayModeType PlayMode { get; set; }
        public Guid? InitialPlayerId { get; set; }
    }
}
