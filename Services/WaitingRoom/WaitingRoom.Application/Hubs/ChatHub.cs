
using Microsoft.AspNetCore.SignalR;
using WaitingRoom.Application.Handlers;

namespace Chat.Application.Hubs;

public class ChatHub : Hub
{
    public async Task Send(string message)
    {
        Console.WriteLine(Context.ConnectionId);
        await Clients.All.SendAsync("Receive", "response" + message);
    }
    
    // for client side
    public async Task GetConnectionId()
    {
        await Clients.Caller.SendAsync("GettingConnId", Context.ConnectionId);
    }

    public async Task AddToSpecialHub(string userId, string roomId)
    {
        await Groups.AddToGroupAsync(userId, roomId);
        await Clients.Group(roomId).SendAsync("Notify", $"{userId} вошел в чат");
    }
}