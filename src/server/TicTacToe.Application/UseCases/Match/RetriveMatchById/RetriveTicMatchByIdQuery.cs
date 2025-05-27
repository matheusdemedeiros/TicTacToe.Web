using MediatR;

namespace TicTacToe.Application.UseCases.Match.RetriveMatchById
{
    public record RetriveTicMatchByIdQuery : IRequest<RetrieveTicMatchByIdResponse>
    {
        public Guid MatchId { get; set; }
    }
}
