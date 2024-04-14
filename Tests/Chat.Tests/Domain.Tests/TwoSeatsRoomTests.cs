using Chat.Domain.Aggregates.Room;
using Chat.Domain.ValueObjects;

namespace Domain.Tests;

public class TwoSeatsRoomTests
{
    [Fact]
    public void HandleChatRoom_WhenTwoEqualUserAddToChat_ShouldBeOneUserInChat()
    {
        ChatUser peer1 =
            new ChatUser(Guid.NewGuid().ToString(), "mail@mail.ru", "12312412341", 0, new HashSet<string>());
        
        ChatUser peer2 =
            new ChatUser(Guid.NewGuid().ToString(), "mail@mail.ru", "9999999", 0, new HashSet<string>());

        TwoSeatsRoom room = new TwoSeatsRoom(Guid.NewGuid().ToString(), new(), DateTime.UtcNow);
        
        room.AddPeer(peer1);
        Assert.False(room.IsFullRoom());
        Assert.Single(room.PeerLinks);

        room.AddPeer(peer1);
        Assert.False(room.IsFullRoom());
        Assert.Single(room.PeerLinks);

        room.AddPeer(peer2);
        Assert.False(room.IsFullRoom());

        Assert.Single(room.PeerLinks);
    }
    
    [Fact]
    public void HandleChatRoom_WhenOneUserJoinTwice_ShouldBeTwoUserInChat()
    {
        ChatUser peer1 =
            new ChatUser(Guid.NewGuid().ToString(), "mail@mail.ru", "12312412341", 0, new HashSet<string>());
        
        ChatUser peer2 =
            new ChatUser(Guid.NewGuid().ToString(), "mail2@mail.ru", "9999999", 0, new HashSet<string>());

        TwoSeatsRoom room = new TwoSeatsRoom(Guid.NewGuid().ToString(), new(), DateTime.UtcNow);
        
        room.AddPeer(peer1);
        Assert.False(room.IsFullRoom());

        room.AddPeer(peer2);
        Assert.True(room.IsFullRoom());

        room.AddPeer(peer2);
        Assert.True(room.IsFullRoom());
    }
    
    [Fact]
    public void HandleChatRoom_WhenThirdUserJoinRoom_ShouldBeRejectToJoin()
    {
        ChatUser peer1 =
            new ChatUser(Guid.NewGuid().ToString(), "mail@mail.ru", "12312412341", 0, new HashSet<string>());
        
        ChatUser peer2 =
            new ChatUser(Guid.NewGuid().ToString(), "mail2@mail.ru", "9999999", 0, new HashSet<string>());

        ChatUser peer3 =
            new ChatUser(Guid.NewGuid().ToString(), "mail3@mail.ru", "9999999", 0, new HashSet<string>());
        
        TwoSeatsRoom room = new TwoSeatsRoom(Guid.NewGuid().ToString(), new(), DateTime.UtcNow);
        
        room.AddPeer(peer1);
        room.AddPeer(peer2);
        
        room.AddPeer(peer3);
        Assert.True(room.IsFullRoom());
        Assert.DoesNotContain(room.PeerLinks, ul => ul.Email == peer3.Email);
        Assert.Contains(room.PeerLinks, ul => ul.Email == peer2.Email);
        Assert.Contains(room.PeerLinks, ul => ul.Email == peer1.Email);

    }
}