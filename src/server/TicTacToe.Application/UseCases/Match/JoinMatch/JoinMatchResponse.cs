using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Application.UseCases.Match.JoinMatch
{
    public record JoinMatchResponse
    {
        public Guid MatchId { get; set; }

        public TicBoardCell[][] Board { get; set; }

        public TicMatchState State { get; set; }

        public Guid CurrentPlayerId { get; set; }

        public string CurrentPlayerSymbol { get; set; }

        public Guid TicPlayerWithXSymbolId { get; set; }
        public Guid TicPlayerWithOSymbolId { get; set; }
    }
}