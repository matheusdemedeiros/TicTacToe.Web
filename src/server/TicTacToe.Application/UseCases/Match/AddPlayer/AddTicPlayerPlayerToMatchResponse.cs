namespace TicTacToe.Application.UseCases.Match.AddPlayer
{
    public record AddTicPlayerPlayerToMatchResponse
    {
        /// <summary>
        /// Identifier for the match to which the player was added.
        /// </summary>
        public Guid MatchId { get; internal set; }
        public Guid PlayerId { get; internal set; }
        public string PlayerName { get; internal set; }
        public string Nickname { get; internal set; }
    }
}
