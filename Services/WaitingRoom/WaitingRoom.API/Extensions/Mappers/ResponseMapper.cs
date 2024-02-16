using Chat.Application.Models;
using WaitingRoom.Application.Responses;
using WaitingRoom.HttpModels.HttpResponse;

namespace WaitingRoom.API.Extensions.Mappers;

public static class ResponseMapper
{
    public static UserJoinResponse ToHttpResponse(this RoomJoinInfo user)
    {
        return new UserJoinResponse {
            IsHost = user.IsHost,
            RoomId = user.roomId,
            User = new UserInfo()
            {
                ConnectionId = user.ChatUser.ConnectionId,
                Email = user.ChatUser.Email
            }
        };
    }
    
    public static List<RoomInfoResponse> ToHttpRoomList(this List<RoomInfo> rooms)
    {
        return rooms.Select(r => new RoomInfoResponse()
        {
            Id = r.Id,
            Host = new() {ConnectionId = r.Host.ConnectionId, Email = r.Host.Email},
            Participant = new() {ConnectionId = r.Participant.ConnectionId, Email = r.Participant.Email},
            IsExist = r.IsExist
        }).ToList();
    }
    
    public static RoomInfoResponse ToHttpRoom(this RoomInfo r)
    {
        return  new RoomInfoResponse
        {
            Id = r.Id,
            Host = new() {ConnectionId = r.Host.ConnectionId, Email = r.Host.Email},
            Participant = new() {ConnectionId = r.Participant.ConnectionId, Email = r.Participant.Email},
            IsExist = r.IsExist
        };
    }
}