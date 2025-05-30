using MediatR;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.Application.UseCases.Match.MakeMove
{
    public class MakePlayerMoveHandler : IRequestHandler<MakePlayerMoveCommand, MakePlayerMoveResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MakePlayerMoveHandler(ITicMatchRepository ticMatchRepository, IUnitOfWork unitOfWork)
        {
            _ticMatchRepository = ticMatchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MakePlayerMoveResponse> Handle(MakePlayerMoveCommand request, CancellationToken cancellationToken)
        {
            var matchId = Guid.Parse(request.MatchId);
            var playerId = Guid.Parse(request.PlayerId);
            var match = await _ticMatchRepository.RetrieveByIdAsync(matchId);

            if (match == null)
            {
                throw new Exception("Match not found.");
            }

            if (match.CurrentPlayer == null || match.CurrentPlayer.Id != playerId)
            {
                throw new Exception("It's not your turn.");
            }

            match.MakePlay(match.CurrentPlayer.Symbol, request.CellRow, request.CellCol);

            await _ticMatchRepository.UpdateAsync(match);
            await _unitOfWork.CommitAsync();

            var response = new MakePlayerMoveResponse
            {
                MatchId = match.Id,
                Board = match.Board.Board,
                State = match.State,
                CurrentPlayerId = match.CurrentPlayer != null ? match.CurrentPlayer.Id : Guid.Empty,
                CurrentPlayerSymbol = match.CurrentPlayer != null ? match.CurrentPlayer.Symbol : string.Empty
            };

            return response;
        }
    }
}
