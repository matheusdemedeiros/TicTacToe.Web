using MediatR;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.Application.UseCases.Match.JoinMatch
{
    public class JoinMatchHandler : IRequestHandler<JoinMatchCommand, JoinMatchResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;

        public JoinMatchHandler(ITicMatchRepository ticMatchRepository)
        {
            _ticMatchRepository = ticMatchRepository;
        }

        public async Task<JoinMatchResponse> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.MatchId, out var matchGuid))
            {
                throw new Exception("Invalid match ID format.");
            }

            var match = await _ticMatchRepository.RetrieveByIdAsync(matchGuid);

            if (match == null)
            {
                throw new Exception("Match not found.");
            }

            var response = new JoinMatchResponse
            {
                MatchId = match.Id,
                Board = match.Board.Board,
                State = match.State,
                CurrentPlayerId = match.CurrentPlayer != null ? match.CurrentPlayer.Id : Guid.Empty,
                CurrentPlayerSymbol = match.CurrentPlayer != null ? match.CurrentPlayer.Symbol : string.Empty
            };

            return response;
        }
    }
}
