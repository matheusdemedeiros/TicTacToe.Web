using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;

namespace TicTacToe.Application.UseCases.Match.MakeMove
{
    public record MakePlayerMoveCommand : IRequest<TicMatchStateResponse>
    {
        public string MatchId { get; set; }
        public string PlayerId { get; set; }
        public int CellRow { get; set; }
        public int CellCol { get; set; }
    }
}
