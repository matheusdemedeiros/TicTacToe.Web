using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Application.UseCases.Match.RetriveMatchById
{
    public record RetrieveTicMatchByIdResponse
    {
        public bool Found { get; set; }
        public TicMatchState? TicMatchState { get; set; }
        public int PlayerNumbers { get; set; }
        public Guid MatchId { get; set; }
    }
}
