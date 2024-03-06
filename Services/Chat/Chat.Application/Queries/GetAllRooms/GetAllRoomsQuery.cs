using Chat.Domain.Entities;
using MediatR;

namespace Chat.Application.Queries.GetAllRooms;

public class GetAllRoomsQuery : IRequest<List<TwoSeatsRoom>>
{
    
}