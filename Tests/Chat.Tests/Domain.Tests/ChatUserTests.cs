using Chat.Domain.Aggregates.Room;

namespace Domain.Tests;

public class ChatUserTests
{
    [Fact]
    public void HandleChatUserHistory_WhenTwoEqualUserAddToHistory_ShouldBeOneUserInHistory()
    {
        ChatUser chatUser =
            new ChatUser(Guid.NewGuid().ToString(), "mail@.ru", "12312412341", 0, new HashSet<string>());
        
        ChatUser peerUser =
            new ChatUser(Guid.NewGuid().ToString(), "test@.ru", "9999999", 0, new HashSet<string>());
        
        ChatUser peerUser2 =
            new ChatUser(Guid.NewGuid().ToString(), "test@.ru", "9999999", 0, new HashSet<string>());
        
        // Assert.False(chatUser.CheckInHistory(peerUser.Email));
        Assert.True(chatUser.CheckInHistory(peerUser.Email));
        
        chatUser.AddPeerToHistory(peerUser);
        Assert.True(chatUser.CheckInHistory(peerUser.Email));
        chatUser.AddPeerToHistory(peerUser2);
        Assert.True(chatUser.CheckInHistory(peerUser2.Email));
        
        Assert.Single(chatUser.PreviousParticipantEmails);
    }
    
    [Fact]
    public void HandleChatUserHistory_WhenAdd4NewPeers_TheFirstPeerShouldBeDeleted()
    {
        ChatUser chatUser =
            new ChatUser(Guid.NewGuid().ToString(), "mail@.ru", "12312412341", 0, new HashSet<string>());
        
        ChatUser peerUser =
            new ChatUser(Guid.NewGuid().ToString(), "test1@.ru", "9999999", 0, new HashSet<string>());
        
        ChatUser peerUser2 =
            new ChatUser(Guid.NewGuid().ToString(), "tes2@.ru", "9999999", 0, new HashSet<string>());
        
        ChatUser peerUser3 =
            new ChatUser(Guid.NewGuid().ToString(), "test3@.ru", "9999999", 0, new HashSet<string>());
        
        ChatUser peerUser4 =
            new ChatUser(Guid.NewGuid().ToString(), "test4@.ru", "9999999", 0, new HashSet<string>());
        
        Assert.False(chatUser.CheckInHistory(peerUser.Email));
        
        chatUser.AddPeerToHistory(peerUser);
        Assert.True(chatUser.CheckInHistory(peerUser.Email));
        Assert.False(chatUser.CheckInHistory(peerUser2.Email));
        
        chatUser.AddPeerToHistory(peerUser2);
        chatUser.AddPeerToHistory(peerUser3);
        chatUser.AddPeerToHistory(peerUser4);
        
        Assert.Equal(3, chatUser.PreviousParticipantEmails.Count);
        
        Assert.DoesNotContain(chatUser.PreviousParticipantEmails, email => email == peerUser.Email);
        Assert.Contains(chatUser.PreviousParticipantEmails, email => email == peerUser2.Email);
        Assert.Contains(chatUser.PreviousParticipantEmails, email => email == peerUser3.Email);
        Assert.Contains(chatUser.PreviousParticipantEmails, email => email == peerUser4.Email);
    }
}