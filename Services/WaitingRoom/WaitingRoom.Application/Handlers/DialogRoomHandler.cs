using Chat.Application.Requests;
using Chat.Infrastructure.Models;
using WaitingRoom.Application.Responses;
using WaitingRoom.Core.Repositories;
using WaitingRoom.Infrastructure.Responses;

namespace WaitingRoom.Application.Handlers;

public class DialogRoomHandler
{
    private readonly IDialogRoomRepository _repository;

    public DialogRoomHandler(IDialogRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task<MeetingsData> GetAllMeetings()
    {
        var result = await _repository.GetAllMeetings();

        return result;
    }

    public async Task<RoomInfo> GetMeetingById(string id)
    {
        var room = _repository.FindRoomById(id);

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
            Listeners = room.Talkers,
            IsExist = true
        };
    }

    public async Task<ZoomRoomCreated> JoinFreeRoom(UserRequest user)
    {
        var result = await _zoomClient.CreateRoom(token);

        if (String.IsNullOrEmpty(result.Id))
        {
            return new ZoomRoomCreated() { IsCreated = false };
        }

        _repository.CreateRoom(result.Id, result.JoinUrl);

        result.IsCreated = true;
        return result;
    }
    
    
}