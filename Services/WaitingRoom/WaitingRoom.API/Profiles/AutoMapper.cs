using AutoMapper;
using Chat.Application.Requests;
using WaitingRoom.API.HttpRequests;

namespace WaitingRoom.API.Profiles;

public class RequestMapper : Profile {
    public RequestMapper() {
        CreateMap<UserJoinRequest, UserAdd>();
        CreateMap<UserLeaveRequest, UserLeave>();
    }
}