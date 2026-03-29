using TicTacToe.Domain.Entities.MatchModule;

namespace TicTacToe.Application.UseCases.Match.Shared
{
    public class TicMatchStateResponse
    {
        public Guid MatchId { get; set; }
        public string ShortCode { get; set; }
        public TicBoardCell[][] Board { get; set; }
        public TicMatchState State { get; set; }
        public Guid CurrentPlayerId { get; set; }
        public string CurrentPlayerSymbol { get; set; }
        public Guid TicPlayerWithXSymbolId { get; set; }
        public Guid TicPlayerWithOSymbolId { get; set; }
        public string? PlayerXNickName { get; set; }
        public string? PlayerONickName { get; set; }
        public bool IsFinished { get; set; }
        public bool IsTie { get; set; }
        public bool IsAbandoned { get; set; }
        public string? WinnerSymbol { get; set; }
        public Guid? WinnerPlayerId { get; set; }

        public static TicMatchStateResponse FromMatch(TicMatch match)
        {
            var playerX = match.Players.FirstOrDefault(p => p.Symbol == "X");
            var playerO = match.Players.FirstOrDefault(p => p.Symbol == "O");
            var isTie = match.TicScore?.Tie ?? false;
            var isFinished = match.State == TicMatchState.FINISHED;
            var hasWinner = isFinished && !isTie && match.TicScore?.WinningPlayer != null;
            var isAbandoned = isFinished && !isTie && match.TicScore?.WinningPlayer == null;

            return new TicMatchStateResponse
            {
                MatchId = match.Id,
                ShortCode = match.ShortCode,
                Board = match.Board.Board,
                State = match.State,
                CurrentPlayerId = match.CurrentPlayer?.Id ?? Guid.Empty,
                CurrentPlayerSymbol = match.CurrentPlayer?.Symbol ?? string.Empty,
                TicPlayerWithXSymbolId = playerX?.Id ?? Guid.Empty,
                TicPlayerWithOSymbolId = playerO?.Id ?? Guid.Empty,
                PlayerXNickName = playerX?.NickName,
                PlayerONickName = playerO?.NickName,
                IsFinished = isFinished,
                IsTie = isTie,
                IsAbandoned = isAbandoned,
                WinnerSymbol = hasWinner ? match.TicScore!.WinningSymbol : null,
                WinnerPlayerId = hasWinner ? match.TicScore!.WinningPlayer!.Id : null
            };
        }
    }
}
