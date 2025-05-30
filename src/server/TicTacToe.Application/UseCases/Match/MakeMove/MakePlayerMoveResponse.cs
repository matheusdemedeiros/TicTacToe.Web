using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Application.UseCases.Match.MakeMove
{
    public class MakePlayerMoveResponse
    {
        public Guid MatchId { get; set; }

        public TicBoardCell[][] Board { get; set; }

        public TicMatchState State { get; set; }

        public Guid CurrentPlayerId { get; set; }

        public string CurrentPlayerSymbol { get; set; }
    }
}