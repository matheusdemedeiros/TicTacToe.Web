using MediatR;
using TicTacToe.Domain.Entities.MatchModule;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.Application.UseCases.Match.CreatePlayer
{
    internal class CreateTicPlayerHandler : IRequestHandler<CreateTicPlayerCommand, CreateTicPlayerResponse>
    {
        private readonly ITicPlayerRepository _ticPlayerRepository;

        public CreateTicPlayerHandler(ITicPlayerRepository ticPlayerRepository)
        {
            _ticPlayerRepository = ticPlayerRepository;
        }

        public async Task<CreateTicPlayerResponse> Handle(CreateTicPlayerCommand request, CancellationToken cancellationToken)
        {
            var exists = await _ticPlayerRepository.HasAnyWithConditionAsync(p => p.NickName.Equals(request.nickName, StringComparison.InvariantCultureIgnoreCase));

            if (exists)
            {
                throw new Exception("Player with this nickname already exists.");
            }

            var ticPlayer = new TicPlayer(request.name, request.nickName, "X");

            await _ticPlayerRepository.CreateAsync(ticPlayer);

            var response = new CreateTicPlayerResponse(ticPlayer.Id);

            return response;
        }
    }
}
