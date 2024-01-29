using Chat.Application.Hubs;
using Chat.Application.Requests;
using Microsoft.AspNetCore.SignalR;
using WaitingRoom.Application.Responses;
using WaitingRoom.Core.Models;
using WaitingRoom.Core.Repositories;

namespace WaitingRoom.Application.Handlers;

public class DialogRoomHandler
{
    private readonly IDialogRoomRepository _repository;
    private readonly ChatHub _hub;

    public DialogRoomHandler(IDialogRoomRepository repository, ChatHub hub)
    {
        _repository = repository;
        _hub = hub;
    }
    
    public async Task<RoomJoinInfo> JoinFreeRoom(UserAdd userAdd)
    {
        var anyRoom = await _repository.CanConnectToAnyRoom();

        if (anyRoom == null)
        {
            anyRoom = await _repository.CreateRoom();
        }
        
        // join user to found-free room (in database)
        var response = await _repository.JoinRoom(new UserInfo()
        {
            Email = userAdd.Email,
            ConnectionId = userAdd.ConnectionId
        }, anyRoom.Id);

        // add user to text-hub (for signalR)
        await _hub.AddToSpecialHub(userAdd.ConnectionId, response.Id); 

        if (response.Participant == null)
        {
            return new()
            {
                IsHost = true,
                roomId = response.Id,
                User = response.Host
            };
        }
        return new()
        {
            IsHost = false,
            roomId = response.Id,
            User = response.Participant
        };
    }

    public async Task<List<RoomGetInfo>> GetAllMeetings()
    {
        var response = await _repository.GetAllRooms();

        return response.Select(room =>
            new RoomGetInfo()
            {
                Id = room.Id,
                Host = room.Host,
                Participant = room.Participant
            }).ToList();
    }

    public async Task<RoomInfo> GetRoomById(string id)
    {
        var room = await _repository.FindRoomById(id);

        if (room == null)
        {
            return new()
            {
                IsExist = false
            };
        }

        return new()
        {
            Id = room.Id,
            Host = room.Host,
            Participant = room.Participant,
            IsExist = true
        };
    }

    public async Task LeaveRoom(UserLeave user)
    {
        var response = await _repository.LeaveRoom(user.RoomId,
            new UserInfo()
            {
                Email = user.Email
            });

        await _hub.DisbandHub(response?.Host?.ConnectionId, response?.Participant?.ConnectionId, response!.Id);
    }

    public async Task<bool> UserCanConnect()
    {
        var response = await _repository.CanConnectToAnyRoom() != null;

        return response;
    }
}