using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.AbandonMatch
{
    public class AbandonMatchHandler : IRequestHandler<AbandonMatchCommand, TicMatchStateResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AbandonMatchHandler(ITicMatchRepository ticMatchRepository, IUnitOfWork unitOfWork)
        {
            _ticMatchRepository = ticMatchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TicMatchStateResponse> Handle(AbandonMatchCommand request, CancellationToken cancellationToken)
        {
            var matchId = Guid.Parse(request.MatchId);
            var match = await _ticMatchRepository.RetrieveByIdAsync(matchId);

            if (match == null)
            {
                throw new DomainException("Match not found.");
            }

            match.Abandon();

            await _ticMatchRepository.UpdateAsync(match);
            await _unitOfWork.CommitAsync();

            return TicMatchStateResponse.FromMatch(match);
        }
    }
}
