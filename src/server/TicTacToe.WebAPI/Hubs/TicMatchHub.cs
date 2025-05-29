using Microsoft.AspNetCore.SignalR;
using TicTacToe.Domain.Interfaces.MatchModule;

namespace TicTacToe.WebAPI.Hubs
{
    public class TicMatchHub : Hub
    {
        private const string GroupPrefix = "ticmatch";
        private readonly ITicMatchRepository _ticMatchRepository;

        public TicMatchHub(ITicMatchRepository ticMatchRepository)
        {
            _ticMatchRepository = ticMatchRepository;
        }

        public async Task JoinMatchAsync(string matchId)
        {
            try
            {

                if (!Guid.TryParse(matchId, out var matchGuid))
                {
                    throw new HubException("Invalid match ID format.");
                }

                var groupName = $"{GroupPrefix}-{matchId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                var match = await _ticMatchRepository.RetrieveByIdAsync(matchGuid);
                if (match == null)
                {
                    throw new HubException("Match not found.");
                }

                await Clients.Group(groupName).SendAsync("TicPlayerJoined", match);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Aqui você pode implementar lógica futura para remoção de grupos ou logs
            await base.OnDisconnectedAsync(exception);
        }
    }
}
