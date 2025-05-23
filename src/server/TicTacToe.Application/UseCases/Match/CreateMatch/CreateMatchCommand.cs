using MediatR;

namespace TicTacToe.Application.UseCases.Match.CreateMatch
{
    public record CreateMatchCommand
        (
            string FullName,
            string NickName,
            string PlayMode,
            string MatchType,
            string MatchId
        ) : IRequest<CreateMatchResponse>
    {
    }
}
