using MediatR;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.Application.UseCases.Match.JoinMatch
{
    public class JoinMatchHandler : IRequestHandler<JoinMatchCommand, JoinMatchResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JoinMatchHandler(ITicMatchRepository ticMatchRepository, IUnitOfWork unitOfWork)
        {
            _ticMatchRepository = ticMatchRepository;
            _unitOfWork = unitOfWork;
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

            if (match.Players.Count == 2)
            {
                match.StartMatch();
                await _unitOfWork.CommitAsync();
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
