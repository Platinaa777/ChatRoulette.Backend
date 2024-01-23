using Chat.Infrastructure.Models;
using WaitingRoom.Application.Responses;
using WaitingRoom.Core.Repositories;
using WaitingRoom.Infrastructure.Communication;
using WaitingRoom.Infrastructure.Responses;

namespace WaitingRoom.Application.Handlers;

public class DialogRoomHandler
{
    private readonly IDialogRoomRepository _repository;
    private readonly ZoomClient _zoomClient;

    public DialogRoomHandler(IDialogRoomRepository repository, ZoomClient zoomClient)
    {
        _repository = repository;
        _zoomClient = zoomClient;
    }

    public async Task<MeetingsData> GetAllMeetings(string token)
    {
        var result = await _zoomClient.GetAllMeetings(token);

        return result;
    }

    public async Task<ZoomInfo> GetMeetingById(string id, string token)
    {
        var room = _repository.FindRoomById(id);

        if (room == null)
        {
            return new()
            {
                IsValid = false
            };
        }
        
        // todo: automapper
        return new()
        {
            Id = room.Id,
            HostUrl = room.ConnectionString,
            IsValid = true,
        };
    }

    public async Task<ZakTokenInfo> GetZakToken(string token)
    {
        var response = await _zoomClient.GetZakToken(token);
        var zakTokenResponse = new ZakTokenInfo();
        
        zakTokenResponse.IsValid = true;
        zakTokenResponse.Signature = response;
        
        if (response == null)
        {
            zakTokenResponse.IsValid = false;
        }

        return zakTokenResponse;
    }

    public async Task<ZoomRoomCreated> CreateRoom(string token)
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