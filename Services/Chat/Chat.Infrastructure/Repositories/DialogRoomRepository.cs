using Chat.Core.Models;
using Chat.Core.Repositories;

namespace Chat.Infrastructure.Repositories;

public class DialogRoomRepository : IDialogRoomRepository
{
    private List<TwoSeatsRoom> _meetings = new();
    
    public string CreateRoom(string id, string connectionString)
    {
        var meeting = new TwoSeatsRoom()
        {
            Id = id,
            ConnectionString = connectionString,
            Duration = 60,
        };
        
        _meetings.Add(meeting);
        return id;
    }

    public string JoinRoom(string roomId, string userEmail)
    {
        foreach (var room in _meetings)
        {
            if (room.Id == roomId)
            {
                room.Talkers.Add(userEmail);
                room.IsInitial = false;
                return room.ConnectionString;
            }
        }
        
        return String.Empty;
    }

    public bool LeaveRoom(string roomId, string userEmail)
    {
        foreach (var meeting in _meetings)
        {
            if (meeting.Id == roomId && meeting.Talkers.Contains(userEmail))
            {
                meeting.Talkers.Remove(userEmail);
                return true;
            }
        }

        return false;
    }

    public TwoSeatsRoom? CanConnectToAnyRoom()
    {
        foreach (var room in _meetings)
        {
            if (room.Talkers.Count == 1)
            {
                return room;
            }
        }

        return null;
    }
}