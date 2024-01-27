using Chat.Application.Requests;
using WaitingRoom.Application.Responses;
using WaitingRoom.Core.Models;
using WaitingRoom.Core.Repositories;

namespace WaitingRoom.Application.Handlers;

public class DialogRoomHandler
{
    private readonly IDialogRoomRepository _repository;

    public DialogRoomHandler(IDialogRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoomGetInfo>> GetAllMeetings()
    {
        var response = await _repository.GetAllRooms();

        return response.Select(room => 
            new RoomGetInfo() {
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

    public async Task<RoomGetInfo> JoinFreeRoom(UserAdd userAdd)
    {
        var anyRoom = await _repository.CanConnectToAnyRoom();

        if (anyRoom == null)
        {
            anyRoom = await _repository.CreateRoom();
        }
        
        var response = await _repository.JoinRoom(new UserInfo()
        {
            Email = userAdd.Email
        }, anyRoom.Id);

        return new RoomGetInfo()
        {
            Id = response.Id,
            Talkers = response.Talkers
        };
    }

    public async Task<bool> LeaveRoom(UserLeave user)
    {
        return await _repository.LeaveRoom(user.RoomId,
            new UserInfo()
            {
                Email = user.Email
            });
    }

    public async Task<bool> UserCanConnect()
    {
        var response = await _repository.CanConnectToAnyRoom() != null;

        return response;
    }
}