using MediatR;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.CreatePlayer
{
    public class CreateTicPlayerHandler : IRequestHandler<CreateTicPlayerCommand, CreateTicPlayerResponse>
    {
        private readonly ITicPlayerRepository _ticPlayerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicPlayerHandler(ITicPlayerRepository ticPlayerRepository, IUnitOfWork unitOfWork)
        {
            _ticPlayerRepository = ticPlayerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTicPlayerResponse> Handle(CreateTicPlayerCommand request, CancellationToken cancellationToken)
        {
            var existingPlayer = await _ticPlayerRepository.RetrieveByConditionAsync(p => p.NickName.Equals(request.nickName));

            if (existingPlayer != null)
            {
                return new CreateTicPlayerResponse(existingPlayer.Id);
            }

            var ticPlayer = new TicPlayer(request.name, request.nickName);

            await _ticPlayerRepository.CreateAsync(ticPlayer);
            await _unitOfWork.CommitAsync();

            return new CreateTicPlayerResponse(ticPlayer.Id);
        }
    }
}
