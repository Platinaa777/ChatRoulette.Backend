using Chat.Application.Requests;
using WaitingRoom.HttpModels.HttpRequests;

namespace WaitingRoom.API.Extensions.Mappers;

public static class RequestMapper
{
    public static UserAdd ToUserAdd(this UserJoinRequest user)
    {
        return new UserAdd()
        {
            Email = user.Email,
            ConnectionId = user.ConnectionId
        };
    }
    
    public static UserLeave ToUserLeave(this UserLeaveRequest user)
    {
        return new UserLeave
        {
            RoomId = user.RoomId,
            Email = user.Email,
            ConnectionId = user.ConnectionId
        };
    }
}