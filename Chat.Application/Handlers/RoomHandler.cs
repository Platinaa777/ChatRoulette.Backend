using Chat.Core.Repositories;
using Chat.Core.Secrets;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;

namespace Chat.Application.Handlers;

public class RoomHandler
{
    private readonly TwilioSettings _twilioSettings;
    private readonly IRoomRepository _repository;

    public RoomHandler(TwilioSettings settings, IRoomRepository repository)
    {
        _twilioSettings = settings;
        _repository = repository;
        TwilioClient.Init(_twilioSettings.AccountSid,_twilioSettings.AuthToken);
    }
    
    public async Task<string> CreateRoom()
    {
        var nameRoom = Guid.NewGuid().ToString();
        var room = await RoomResource.CreateAsync(uniqueName: nameRoom, type: RoomResource.RoomTypeEnum.Go);

        await _repository.CreateRoom(room.Sid);
        return room.Sid;
    }

    public async Task<string> JoinRoom()
    {
        var room = await _repository.GetFreeRoom();
        if (string.IsNullOrEmpty(room))
        {
            return "";
        }
        
        var token = await CreateAccessTokenForRoom(room);
        var isConnected = await _repository.ConnectToRoom(room);

        if (!isConnected)
        {
            return "";
        }
        
        return token;
    }
    
    private async Task<string> CreateAccessTokenForRoom(string room)
    {
        string identity = Guid.NewGuid().ToString();
        var grant = new VideoGrant();
        
        grant.Room = room;

        var grants = new HashSet<IGrant> { grant };

        var token = new Token(
            _twilioSettings.AccountSid,
            _twilioSettings.ApiKey,
            _twilioSettings.ApiSecret,
            identity: identity,
            grants: grants);

        return token.ToJwt();
    }

}