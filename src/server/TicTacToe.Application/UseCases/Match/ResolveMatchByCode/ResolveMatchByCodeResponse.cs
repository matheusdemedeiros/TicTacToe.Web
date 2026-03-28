namespace TicTacToe.Application.UseCases.Match.ResolveMatchByCode
{
    public record ResolveMatchByCodeResponse
    {
        public Guid MatchId { get; set; }
        public string ShortCode { get; set; }
    }
}
