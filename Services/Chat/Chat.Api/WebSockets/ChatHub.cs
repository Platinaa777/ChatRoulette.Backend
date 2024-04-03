using Chat.Application.Commands.CloseRoomCommand;
using Chat.Application.Commands.ConnectUser;
using Chat.Application.Queries.GetAllRooms;
using Chat.Application.Queries.GetRoom;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;
using Chat.HttpModels.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.WebSockets;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task GetId()
    {
        await Clients.Client(Context.ConnectionId).SendAsync("id", Context.ConnectionId);
    }
    
    // for client side (for messaging)
    public async Task SendMessageInRoom(string message, string roomId, string email)
    {
        await Clients.Groups(roomId).SendAsync("onReceiveMessage", 
            $"{email}: {message}");
    }

    public async Task FindRoom(string connectionId, string email)
    {
        var findRoomCommand = new ConnectUserCommand() { ConnectionId = connectionId, Email = email };

        UserJoinResponse response = await _mediator.Send(findRoomCommand);
        // add client to special group
        await Groups.AddToGroupAsync(Context.ConnectionId, response.RoomId!);
        // Console.WriteLine($"Client {Context.ConnectionId} was joined in room {response?.RoomId}");
        if (response!.CreateOffer)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("PeerConnection",
                response.RoomId,
                "",
                "offer");
        }
        else
        {
            await Clients.Client(Context.ConnectionId).SendAsync("PeerConnection",
                response.RoomId,
                "", // offer || answer
                ""); // command
        }
    }

    public async Task OnPeerOffer(string roomId, string offer)
    {
        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null)
            return;

        foreach (var peerLink in room.PeerLinks)
        {
            if (peerLink.ConnectionId != Context.ConnectionId)
            {
                await Clients.Client(peerLink.ConnectionId).SendAsync("PeerConnection",
                    roomId,
                    offer,
                    "answer");
                return;
            }
        }
    }
    
    public async Task OnPeerAnswer(string roomId, string answer)
    {
        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null)
            return;
        
        foreach (var peer in room.PeerLinks)
        {
            if (peer.ConnectionId != Context.ConnectionId)
            {
                await Clients.Client(peer.ConnectionId).SendAsync("PeerConnection",
                    roomId,
                    answer,
                    "confirmAnswer");
                return;
            }
        }
    }
    
    public async Task OnStartRelayIce(string roomId)
    {
        Console.WriteLine($"ON START RELAY ICE {roomId}");
        await Clients.Groups(roomId).SendAsync("PeerConnection",
            roomId,
            "",
            "relay-ice");
    }
    
    public async Task OnIceCandidate(string roomId, string candidates)
    {
        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null)
            return;
        
        Console.WriteLine($"SENDING ICE {roomId}");
        
        foreach (var peer in room.PeerLinks)
        {
            if (peer.ConnectionId != Context.ConnectionId)
            {
                // Console.WriteLine($"From {Context.ConnectionId} IceCandidateTo: {peer}");
                await Clients.Client(peer.ConnectionId).SendAsync("PeerConnection",
                    roomId,
                    candidates,
                    "candidate");
                return;
            }
        }
    }

    public async Task OnNextRoom()
    {
        await SwitchRoom("next-room");
    }
    
    public async Task OnLeaveRoom()
    {
        await SwitchRoom("leave-room");
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await SwitchRoom("leave-room");
    }

    private async Task SwitchRoom(string command)
    {
        var rooms = await _mediator.Send(new GetAllRoomsQuery());
        TwoSeatsRoom? storedRoom = null;
        
        foreach (var room in rooms)
        {
            foreach (var userLink in room.PeerLinks)
            {
                if (userLink.ConnectionId == Context.ConnectionId)
                {
                    storedRoom = room;
                    break;
                }            
            }
        }
        
        if (storedRoom is null)
            return;
        
        // close room
        var response = await _mediator.Send(new CloseRoomCommand()
        {
            RoomId = storedRoom.Id
        });
        
        if (!response)
            return;
        
        // send to peers in group to stop their video tracks
        await Clients.Groups(storedRoom.Id).SendAsync("PeerConnection",
            storedRoom.Id,
            "",
            command);
        
        UserLink? peer1 = null;
        UserLink? peer2 = null;
        if (storedRoom.PeerLinks.Count == 2)
        {
            peer1 = storedRoom.PeerLinks[0];
            peer2 = storedRoom.PeerLinks[1];    
        }
        
        if (peer1 is not null)
        {
            // Console.WriteLine("Disconnected: " + peer1?.ConnectionId);
            await Groups.RemoveFromGroupAsync(peer1.ConnectionId, storedRoom.Id);    
        }
        
        if (peer2 is not null)
        {
            // Console.WriteLine("Disconnected: " + peer2?.ConnectionId);
            await Groups.RemoveFromGroupAsync(peer2.ConnectionId, storedRoom.Id);    
        }
    }
}