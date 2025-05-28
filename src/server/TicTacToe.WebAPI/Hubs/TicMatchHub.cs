using Microsoft.AspNetCore.SignalR;

namespace TicTacToe.WebAPI.Hubs
{
    public class TicMatchHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var matchId = Context.GetHttpContext().Request.Query["matchId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
        }
    }
}
