using MediatR;
using TicTacToe.Application.UseCases.Match.Shared;
using TicTacToe.Domain.Interfaces;
using TicTacToe.Domain.Interfaces.MatchModule;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.Application.UseCases.Match.MakeMove
{
    public class MakePlayerMoveHandler : IRequestHandler<MakePlayerMoveCommand, TicMatchStateResponse>
    {
        private readonly ITicMatchRepository _ticMatchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IComputerMoveCalculator _computerMoveCalculator;

        public MakePlayerMoveHandler(
            ITicMatchRepository ticMatchRepository,
            IUnitOfWork unitOfWork,
            IComputerMoveCalculator computerMoveCalculator)
        {
            _ticMatchRepository = ticMatchRepository;
            _unitOfWork = unitOfWork;
            _computerMoveCalculator = computerMoveCalculator;
        }

        public async Task<TicMatchStateResponse> Handle(MakePlayerMoveCommand request, CancellationToken cancellationToken)
        {
            var matchId = Guid.Parse(request.MatchId);
            var playerId = Guid.Parse(request.PlayerId);
            var match = await _ticMatchRepository.RetrieveByIdAsync(matchId);

            if (match == null)
            {
                throw new DomainException("Match not found.");
            }

            if (match.CurrentPlayer == null || match.CurrentPlayer.Id != playerId)
            {
                throw new DomainException("It's not your turn.");
            }

            match.MakePlay(match.CurrentPlayer.Symbol, request.CellRow, request.CellCol);

            int? computerMoveRow = null;
            int? computerMoveCol = null;

            if (match.IsComputerTurn && match.ComputerDifficulty.HasValue)
            {
                var computerPlayer = match.GetComputerPlayer()!;
                var (row, col) = _computerMoveCalculator.Calculate(
                    match.Board,
                    computerPlayer.Symbol,
                    match.ComputerDifficulty.Value);

                match.MakeComputerPlay(row, col);
                computerMoveRow = row;
                computerMoveCol = col;
            }

            await _ticMatchRepository.UpdateAsync(match);
            await _unitOfWork.CommitAsync();

            var response = TicMatchStateResponse.FromMatch(match);
            response.ComputerMoveRow = computerMoveRow;
            response.ComputerMoveCol = computerMoveCol;
            return response;
        }
    }
}
