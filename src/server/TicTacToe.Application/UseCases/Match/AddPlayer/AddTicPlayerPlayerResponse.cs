namespace TicTacToe.Application.UseCases.Match.AddPlayer
{
    public record AddTicPlayerPlayerResponse
    {
        public Guid MatchId { get; internal set; }
        public Guid PlayerId { get; internal set; }
        public string PlayerName { get; internal set; }
        public string Nickname { get; internal set; }
    }
}
