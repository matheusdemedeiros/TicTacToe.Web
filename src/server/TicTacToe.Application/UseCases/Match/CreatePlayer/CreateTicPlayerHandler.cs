using MediatR;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;

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
            var exists = await _ticPlayerRepository.HasAnyWithConditionAsync(p => p.NickName.Equals(request.nickName));

            if (exists)
            {
                throw new Exception("Player with this nickname already exists.");
            }

            var ticPlayer = new TicPlayer(request.name, request.nickName, "X");

            await _ticPlayerRepository.CreateAsync(ticPlayer);
            await _unitOfWork.CommitAsync();

            var response = new CreateTicPlayerResponse(ticPlayer.Id);

            return response;
        }
    }
}
