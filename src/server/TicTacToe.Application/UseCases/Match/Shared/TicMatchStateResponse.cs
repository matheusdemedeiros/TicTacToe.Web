using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Application.UseCases.Match.Shared
{
    public class TicMatchStateResponse
    {
        public Guid MatchId { get; set; }
        public TicBoardCell[][] Board { get; set; }
        public TicMatchState State { get; set; }
        public Guid CurrentPlayerId { get; set; }
        public string CurrentPlayerSymbol { get; set; }
        public Guid TicPlayerWithXSymbolId { get; set; }
        public Guid TicPlayerWithOSymbolId { get; set; }
        public bool IsFinished { get; set; }
        public bool IsTie { get; set; }
        public string? WinnerSymbol { get; set; }
        public Guid? WinnerPlayerId { get; set; }

        public static TicMatchStateResponse FromMatch(TicMatch match)
        {
            var playerX = match.Players.FirstOrDefault(p => p.Symbol == "X");
            var playerO = match.Players.FirstOrDefault(p => p.Symbol == "O");

            return new TicMatchStateResponse
            {
                MatchId = match.Id,
                Board = match.Board.Board,
                State = match.State,
                CurrentPlayerId = match.CurrentPlayer?.Id ?? Guid.Empty,
                CurrentPlayerSymbol = match.CurrentPlayer?.Symbol ?? string.Empty,
                TicPlayerWithXSymbolId = playerX?.Id ?? Guid.Empty,
                TicPlayerWithOSymbolId = playerO?.Id ?? Guid.Empty,
                IsFinished = match.State == TicMatchState.FINISHED,
                IsTie = match.TicScore?.Tie ?? false,
                WinnerSymbol = match.TicScore?.WinningSymbol,
                WinnerPlayerId = match.TicScore?.WinningPlayer?.Id
            };
        }
    }
}
