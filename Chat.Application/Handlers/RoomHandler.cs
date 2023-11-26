using Chat.Application.Responses;
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

    public async Task<JoinResponse> JoinRoom()
    {
        var room = await _repository.GetFreeRoom();

        if (_repository.IsAllRoomsFull())
        {
            return new JoinResponse()
            {
                IsValid = false,
                Message = "All rooms is filled, join later"
            };
        }

        // size is less than 10 but all rooms in active mode (2 people in room)
        if (_repository.NotAnyRoomToJoin())
        {
            // create new room if the size is less than 10
            room = await CreateRoom();
            await _repository.CreateRoom(room);
        }
        
        var token = await CreateAccessTokenForRoom(room);
        var isConnected = await _repository.ConnectToRoom(room);

        if (!isConnected)
        {
            return new JoinResponse()
            {
                IsValid = false,
                Message = "This Room is filled"
            };
        }
        
        return new JoinResponse{AccessToken= token, RoomName = room, IsValid = true};
    }
    
    private async Task<string> CreateRoom()
    {
        var nameRoom = Guid.NewGuid().ToString();
        var room = await RoomResource.CreateAsync(uniqueName: nameRoom, type: RoomResource.RoomTypeEnum.Go);

        await _repository.CreateRoom(room.Sid);
        return room.Sid;
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