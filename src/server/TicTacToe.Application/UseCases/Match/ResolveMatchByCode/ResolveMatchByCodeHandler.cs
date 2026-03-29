using MediatR;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.ResolveMatchByCode
{
    public class ResolveMatchByCodeHandler : IRequestHandler<ResolveMatchByCodeQuery, ResolveMatchByCodeResponse>
    {
        private readonly ITicMatchRepository _matchRepository;

        public ResolveMatchByCodeHandler(ITicMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<ResolveMatchByCodeResponse> Handle(ResolveMatchByCodeQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ShortCode) || request.ShortCode.Length != 6)
            {
                throw new DomainException("Invalid match code. Must be 6 characters.");
            }

            var code = request.ShortCode.Trim().ToUpperInvariant();
            var match = await _matchRepository.RetrieveByShortCodeAsync(code);

            if (match == null)
            {
                throw new DomainException("Match not found for the given code.");
            }

            return new ResolveMatchByCodeResponse
            {
                MatchId = match.Id,
                ShortCode = match.ShortCode
            };
        }
    }
}
