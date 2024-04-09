namespace ProfileService.Infrastructure.Repos.Implementations.Friend;

public static class FriendInvitationQuery
{
    public const string SqlFindById = $@"
            SELECT id, sender_id SenderId, receiver_id ReceiverId, status InvitationStatus, sent_at SentAtUtc
            FROM invitations
            WHERE id = @Id
        ";
    
    public const string SqlFindByProfileIds = $@"
            SELECT id, sender_id SenderId, receiver_id ReceiverId, status InvitationStatus, sent_at SentAtUtc
            FROM invitations
            WHERE (sender_id = @SenderId and receiver_id = @ReceiverId);
         ";

    public const string SqlAddInvitation = $@"
        INSERT INTO invitations (id, sender_id, receiver_id, status, sent_at)
        VALUES 
            (@Id, @SenderId, @ReceiverId, @InvitationStatus, @SentAtUtc);
        ";
    
    public const string SqlUpdateInvitation = $@"
        UPDATE invitations
        SET
            sender_id = @SenderId,
            receiver_id = @ReceiverId,
            status = @InvitationStatus,
            sent_at = @SentAtUtc
        WHERE id = @Id
        ";
    
    public const string SqlDeleteInvitation = $@"
        DELETE FROM invitations
        WHERE id = @Id
        ";
}