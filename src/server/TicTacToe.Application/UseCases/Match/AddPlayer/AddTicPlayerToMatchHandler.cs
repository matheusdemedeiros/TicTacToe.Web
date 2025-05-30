﻿using MediatR;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.Application.UseCases.Match.AddPlayer
{
    public class AddTicPlayerToMatchHandler : IRequestHandler<AddTicPlayerToMatchCommand, AddTicPlayerToMatchResponse>
    {
        private readonly ITicMatchRepository _matchRepository;
        private readonly ITicPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTicPlayerToMatchHandler(
            ITicMatchRepository matchRepository,
            ITicPlayerRepository playerRepository,
            IUnitOfWork unitOfWork
            )
        {
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddTicPlayerToMatchResponse> Handle(AddTicPlayerToMatchCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PlayerId) || string.IsNullOrEmpty(request.MatchId))
            {
                return null;
            }

            var matchId = Guid.Parse(request.MatchId);
            var playerId = Guid.Parse(request.PlayerId);

            var playerExists = await _playerRepository.HasAnyWithConditionAsync(player => player.Id == playerId);
            if (!playerExists)
            {
                return null;
            }

            var matchExists = await _matchRepository.HasAnyWithConditionAsync(match => match.Id == matchId);
            if (!matchExists)
            {
                return null;
            }

            var match = await _matchRepository.RetrieveByIdAsync(matchId);
            var player = await _playerRepository.RetrieveByIdAsync(playerId);

            match.AddPlayer(player);

            await _unitOfWork.CommitAsync();

            var ticPlayerWithXSymbolId = match.Players.FirstOrDefault(p => p.Symbol == "X")?.Id ?? Guid.Empty;
            var ticPlayerWithOSymbolId = match.Players.FirstOrDefault(p => p.Symbol == "O")?.Id ?? Guid.Empty;

            var response = new AddTicPlayerToMatchResponse
            {
                MatchId = match.Id,
                PlayerId = player.Id,
                PlayerName = player.Name,
                Nickname = player.NickName,
                TicPlayerWithXSymbolId = ticPlayerWithXSymbolId,
                TicPlayerWithOSymbolId = ticPlayerWithOSymbolId
            };

            return response;
        }
    }
}
