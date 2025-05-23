using MediatR;

namespace TicTacToe.Application.UseCases.Match.CreatePlayer
{
    public record CreateTicPlayerCommand(string name, string nickName) : IRequest<CreateTicPlayerResponse>
    {
    }
}
