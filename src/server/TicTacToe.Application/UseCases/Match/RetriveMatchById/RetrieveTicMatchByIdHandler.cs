using MediatR;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.RetriveMatchById
{
    public class RetrieveTicMatchByIdHandler : IRequestHandler<RetriveTicMatchByIdQuery, RetrieveTicMatchByIdResponse>
    {
        private readonly ITicMatchRepository _matchRepository;

        public RetrieveTicMatchByIdHandler(ITicMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<RetrieveTicMatchByIdResponse> Handle(RetriveTicMatchByIdQuery request, CancellationToken cancellationToken)
        {
            var matchId = request.MatchId;

            var matchExists = await _matchRepository.HasAnyWithConditionAsync(match => match.Id == matchId);
            if (!matchExists)
            {
                throw new DomainException("Match not found.");
            }

            var match = await _matchRepository.RetrieveByIdAsync(matchId);

            var response = new RetrieveTicMatchByIdResponse
            {
                Found = true,
                PlayerNumbers = match.Players.Count,
                TicMatchState = match.State,
                MatchId = match.Id
            };

            return response;
        }
    }
}
