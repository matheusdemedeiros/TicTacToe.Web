using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.Rematch
{
    public class RematchHandler : IRequestHandler<RematchCommand, TicMatchStateResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly ITicPlayerRepository _ticPlayerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RematchHandler(ITicMatchRepository ticMatchRepository, ITicPlayerRepository ticPlayerRepository, IUnitOfWork unitOfWork)
        {
            _ticMatchRepository = ticMatchRepository;
            _ticPlayerRepository = ticPlayerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TicMatchStateResponse> Handle(RematchCommand request, CancellationToken cancellationToken)
        {
            var previousMatchId = Guid.Parse(request.PreviousMatchId);
            var previousMatch = await _ticMatchRepository.RetrieveByIdAsync(previousMatchId);

            if (previousMatch == null)
            {
                throw new DomainException("Previous match not found.");
            }

            if (previousMatch.State != TicMatchState.FINISHED)
            {
                throw new DomainException("Previous match is not finished yet.");
            }

            if (previousMatch.Players.Count != 2)
            {
                throw new DomainException("Previous match does not have two players.");
            }

            var newMatch = CreateRematchFromPrevious(previousMatch);

            await _ticMatchRepository.CreateAsync(newMatch);
            await _unitOfWork.CommitAsync();

            return TicMatchStateResponse.FromMatch(newMatch);
        }

        private TicMatch CreateRematchFromPrevious(TicMatch previousMatch)
        {
            if (previousMatch.IsPlayerVsComputer && previousMatch.ComputerDifficulty.HasValue)
            {
                var newMatch = new TicMatch(PlayModeType.PlayerVsComputer, previousMatch.ComputerDifficulty.Value);
                var humanPlayer = previousMatch.Players.FirstOrDefault(p => p.NickName != "CPU")!;
                newMatch.AddPlayer(humanPlayer);
                var computerPlayer = newMatch.AddComputerPlayer();
                _ticPlayerRepository.CreateAsync(computerPlayer);
                newMatch.StartMatch();
                return newMatch;
            }

            var pvpMatch = new TicMatch(previousMatch.PlayMode);
            var previousPlayerX = previousMatch.Players.FirstOrDefault(p => p.Symbol == "X");
            var previousPlayerO = previousMatch.Players.FirstOrDefault(p => p.Symbol == "O");
            pvpMatch.AddPlayer(previousPlayerO!);
            pvpMatch.AddPlayer(previousPlayerX!);
            return pvpMatch;
        }
    }
}
