using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.JoinMatch
{
    public class JoinMatchHandler : IRequestHandler<JoinMatchCommand, TicMatchStateResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JoinMatchHandler(ITicMatchRepository ticMatchRepository, IUnitOfWork unitOfWork)
        {
            _ticMatchRepository = ticMatchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TicMatchStateResponse> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.MatchId, out var matchGuid))
            {
                throw new DomainException("Invalid match ID format.");
            }

            var match = await _ticMatchRepository.RetrieveByIdAsync(matchGuid);

            if (match == null)
            {
                throw new DomainException("Match not found.");
            }

            if (match.State == TicMatchState.NOT_STARTED && match.Players.Count == 2)
            {
                match.StartMatch();
                await _unitOfWork.CommitAsync();
            }

            return TicMatchStateResponse.FromMatch(match);
        }
    }
}
