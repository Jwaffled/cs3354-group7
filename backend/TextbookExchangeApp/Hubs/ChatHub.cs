using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TextbookExchangeApp.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinChannel(int channelId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"channel-{channelId}");
    }

    public async Task LeaveChannel(int channelId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"channel-{channelId}");
    }
}