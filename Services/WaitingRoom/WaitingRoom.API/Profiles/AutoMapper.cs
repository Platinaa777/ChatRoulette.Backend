using AutoMapper;
using Chat.Application.Requests;
using WaitingRoom.API.HttpRequests;

namespace WaitingRoom.API.Profiles;

public class Mapper: Profile {
    public Mapper() {
        CreateMap<UserJoinRequest, UserRequest>();
    }
}