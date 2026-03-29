using MediatR;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Application.UseCases.Match.AbandonMatch;
using TicTacToe.Application.UseCases.Match.JoinMatch;
using TicTacToe.Application.UseCases.Match.MakeMove;
using TicTacToe.Application.UseCases.Match.Rematch;
using TicTacToe.Application.UseCases.Match.Shared;
using TicTacToe.Domain.SharedModule.Exceptions;

namespace TicTacToe.WebAPI.Hubs
{
    public class TicMatchHub : Hub
    {
        private const string GroupPrefix = "ticmatch";
        private IMediator _mediator;

        public TicMatchHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task JoinMatchAsync(JoinMatchCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
            {
                throw new DomainException("Match not found or invalid command.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(Guid.Parse(command.MatchId)));
            await NotifyAllPlayersFromMatchAsync<TicMatchStateResponse>(result, Guid.Parse(command.MatchId), "TicPlayerJoined");
        }

        public async Task MakePlayerMoveAsync(MakePlayerMoveCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
            {
                throw new DomainException("Invalid move or match not found.");
            }
            await NotifyAllPlayersFromMatchAsync<TicMatchStateResponse>(result, Guid.Parse(command.MatchId), "TicPlayerMadeMove");
        }

        public async Task AbandonMatchAsync(AbandonMatchCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
            {
                throw new DomainException("Failed to abandon match.");
            }
            await NotifyAllPlayersFromMatchAsync<TicMatchStateResponse>(result, Guid.Parse(command.MatchId), "TicMatchAbandoned");
        }

        public async Task RematchAsync(RematchCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
            {
                throw new DomainException("Failed to create rematch.");
            }
            await NotifyAllPlayersFromMatchAsync<TicMatchStateResponse>(result, Guid.Parse(command.PreviousMatchId), "TicMatchRematch");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        private async Task NotifyAllPlayersFromMatchAsync<T>(T response, Guid matchId, string clientMethodName)
        {
            await Clients.Group(GetGroupName(matchId)).SendAsync(clientMethodName, response);
        }

        private string GetGroupName(Guid matchId)
        {
            return $"{GroupPrefix}-{matchId}";
        }
    }
}