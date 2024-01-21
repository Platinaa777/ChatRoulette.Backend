using Chat.Core.Repositories;
using Chat.Infrastructure.Communication;
using Chat.Infrastructure.Responses;
using Newtonsoft.Json.Linq;

namespace Chat.Application.Handlers;

public class DialogRoomHandler
{
    private readonly IDialogRoomRepository _repository;
    private readonly ZoomClient _zoomClient;

    public DialogRoomHandler(IDialogRoomRepository repository, ZoomClient zoomClient)
    {
        _repository = repository;
        _zoomClient = zoomClient;
    }

    public async Task<object> GetAllMeetings(string token)
    {
        return await _zoomClient.GetAllMeetingsId(token);
    }

    public async Task<object> GetMeetingById(string id, string token)
    {
        return await _zoomClient.GetInfoAboutRoom(id, token);
    }

    public async Task<object> GetZakToken(string token)
    {
        return await _zoomClient.GetZakToken(token);
    }

    public async Task<ZoomRoomCreated> CreateRoom(string token)
    {
        var result = await _zoomClient.CreateRoom(token);
        
        return result;
    }
    
    
}