﻿using MediatR;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.Application.UseCases.Match.CreateMatch
{
    public class CreateMatchHandler : IRequestHandler<CreateTicMatchCommand, CreateTicMatchResponse>
    {
        private readonly ITicPlayerRepository _ticPlayerRepository;
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMatchHandler(
            ITicPlayerRepository ticPlayerRepository,
            ITicMatchRepository ticMatchRepository,
            IUnitOfWork unitOfWork
            )
        {
            _ticPlayerRepository = ticPlayerRepository;
            _ticMatchRepository = ticMatchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTicMatchResponse> Handle(CreateTicMatchCommand request, CancellationToken cancellationToken)
        {
            var ticMatch = new TicMatch(request.PlayMode);

            await AddInitialPlayerIfExists(request, ticMatch);

            await _ticMatchRepository.CreateAsync(ticMatch);

            await _unitOfWork.CommitAsync();

            var response = new CreateTicMatchResponse(ticMatch.Id);

            return response;
        }

        private async Task AddInitialPlayerIfExists(CreateTicMatchCommand request, TicMatch ticMatch)
        {
            if (string.IsNullOrEmpty(request.InitialPlayerId))
            {
                return;
            }
            var initialPlayerId = Guid.Parse(request.InitialPlayerId);
            if (initialPlayerId == Guid.Empty)
            {
                return;
            }

            var initialPlayer = await _ticPlayerRepository.RetrieveByIdAsync(initialPlayerId);

            if (initialPlayer == null)
            {
                return;
            }

            ticMatch.AddPlayer(initialPlayer);
        }
    }
}
