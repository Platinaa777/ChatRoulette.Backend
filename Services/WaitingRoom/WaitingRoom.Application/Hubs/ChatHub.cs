
using Microsoft.AspNetCore.SignalR;
using WaitingRoom.Application.Handlers;
using WaitingRoom.Core.Repositories;

namespace Chat.Application.Hubs;

public class ChatHub : Hub
{
    private readonly IDialogRoomRepository _repository;

    public ChatHub(IDialogRoomRepository repository)
    {
        _repository = repository;
    }
    // for client side (for messaging)
    public async Task Send(string message, string roomId)
    {
        // Console.WriteLine($"user id = {Context.ConnectionId}; roomId = {roomId}; message = {message}");
        await Clients.Group(roomId).SendAsync("Receive", $"{Context.ConnectionId}: " + message);
    }

    public async Task PingFromPeer(string roomId)
    {
        Console.WriteLine("METHOD: PingFromPeer");
        Console.WriteLine($"user id = {Context.ConnectionId}"); // from peer
        var room = await _repository.FindRoomById(roomId);
        Console.WriteLine($"room id after find: {room.Id}");
        if (room == null) return;
        Console.WriteLine("after null checks");
        await Clients.Client(room.Host.ConnectionId).SendAsync("HostPort", roomId); // to host
    }    
    
    public async Task GetOfferFromHost(string roomId, string offer)
    {
        Console.WriteLine("METHOD: GetOfferFromHostRTC");
        var room = await _repository.FindRoomById(roomId);
        Console.WriteLine($"room id after find: {room.Id}");

        if (room == null) return;
        Console.WriteLine("get response from host");
        Console.WriteLine($"offer: {offer}");
        await Clients.Client(room.Participant.ConnectionId).SendAsync("PeerPort", offer, roomId); // to peer
    }

    public async Task TransportAnswer(string roomId, string answer)
    {
        Console.WriteLine("METHOD: TransportAnswerRTC");
        var room = await _repository.FindRoomById(roomId);
        Console.WriteLine($"room id after find: {room.Id}");

        if (room == null) return;

        Console.WriteLine("get reponse from peer");
        Console.WriteLine($"answer: {answer}");
        await Clients.Client(room.Host.ConnectionId).SendAsync("GetAnswerFromPeer", answer);
    }

    // for client side
    public async Task GetConnectionId()
    {
        Console.WriteLine("call");
        Console.WriteLine(Context.ConnectionId);
        await Clients.Caller.SendAsync("GetId", Context.ConnectionId);
    }

    public async Task AddToSpecialHub(string userId, string roomId)
    {
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