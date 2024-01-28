
using Microsoft.AspNetCore.SignalR;
using WaitingRoom.Application.Handlers;

namespace Chat.Application.Hubs;

public class ChatHub : Hub
{
    // for client side (for messaging)
    public async Task Send(string message, string roomId)
    {
        Console.WriteLine($"user id = {Context.ConnectionId}; roomId = {roomId}; message = {message}");
        await Clients.Group(roomId).SendAsync("Receive", "RESPONSE: " + message);
    }
    
    // for client side
    public async Task GetConnectionId()
    {
        await Clients.Caller.SendAsync("GettingConnId", Context.ConnectionId);
    }

    public async Task AddToSpecialHub(string userId, string roomId)
    {
        Console.WriteLine($"user id = {userId}; roomId = {roomId}");
        await Groups.AddToGroupAsync(userId, roomId);
        await Clients.Group(roomId).SendAsync("Notify", $"{userId} вошел в чат");
    }

    public async Task DisbandHub(string? hostId, string? peerId, string roomId)
    {
        // kick all people in the room
        if (hostId != null)
        {
            await Groups.RemoveFromGroupAsync(hostId, roomId);
        }

        if (peerId != null)
        {
            await Groups.RemoveFromGroupAsync(peerId, roomId);
        }
    }
}