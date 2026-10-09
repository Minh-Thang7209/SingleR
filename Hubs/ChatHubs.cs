using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace SingleR.Hubs;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();

    public async Task JoinChat(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
            return;

        OnlineUsers[Context.ConnectionId] = user.Trim();

        await Clients.All.SendAsync(
            "UsersOnline",
            OnlineUsers.Values.Distinct().ToList()
        );
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (OnlineUsers.TryRemove(Context.ConnectionId, out _))
        {
            await Clients.All.SendAsync(
                "UsersOnline",
                OnlineUsers.Values.Distinct().ToList()
            );
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        string time = DateTime.Now.ToString("HH:mm");

        await Clients.All.SendAsync(
            "ReceiveMessage",
            user,
            message,
            time
        );
    }
}
