using Chat.Core.Models;
using Chat.Core.Repositories;

namespace Chat.Infrastructure.Repositories;

public class DialogRoomRepository : IDialogRoomRepository
{
    private List<TwoSeatsRoom> _meetings = new();
    
    public string CreateRoom(string id)
    {
        var meeting = new TwoSeatsRoom()
        {
            Id = id,
            CountUsersInRoom = 0,
            Duration = 60,
            FirstTalker = null,
            SecondTalker = null
        };
        
        _meetings.Add(meeting);
        return id;
    }

    public string JoinRoom()
    {
        throw new NotImplementedException();
    }

    public string LeaveRoom()
    {
        throw new NotImplementedException();
    }

    public TwoSeatsRoom? CanConnectToAnyRoom()
    {
        foreach (var room in _meetings)
        {
            if (room.CountUsersInRoom == 1)
            {
                return room;
            }
        }

        return null;
    }
}