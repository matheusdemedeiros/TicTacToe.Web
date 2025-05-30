namespace TicTacToe.Application.UseCases.Match.AddPlayer
{
    public record AddTicPlayerToMatchResponse
    {
        /// <summary>
        /// Identifier for the match to which the player was added.
        /// </summary>
        public Guid MatchId { get; internal set; }
        public Guid PlayerId { get; internal set; }
        public string PlayerName { get; internal set; }
        public string Nickname { get; internal set; }
        public Guid TicPlayerWithXSymbolId { get; set; }
        public Guid TicPlayerWithOSymbolId { get; set; }
    }
}
