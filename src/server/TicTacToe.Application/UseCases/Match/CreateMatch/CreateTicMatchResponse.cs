namespace TicTacToe.Application.UseCases.Match.CreateMatch
{
    public record CreateTicMatchResponse()
    {
        public Guid MatchId { get; set; }
        public Guid TicPlayerWithXSymbolId { get; set; }
        public Guid TicPlayerWithOSymbolId { get; set; }
    }
}
