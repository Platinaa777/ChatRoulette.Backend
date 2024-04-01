using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.FriendInvitationAggregate.Enumerations;
using ProfileService.Domain.Models.FriendInvitationAggregate.Errors;
using ProfileService.Domain.Models.FriendInvitationAggregate.Events;
using ProfileService.Domain.Models.FriendInvitationAggregate.Snapshot;
using ProfileService.Domain.Models.Identity;

namespace ProfileService.Domain.Models.FriendInvitationAggregate;

public class FriendInvitation : AggregateRoot<Id>
{
    private FriendInvitation(
        Id id,
        Id senderId,
        Id receiverId,
        InvitationStatus status,
        DateTime sentAt)
    {
        Id = id;
        SenderId = senderId;
        ReceiverId = receiverId;
        Status = status;
        SentAt = sentAt;
    }
    
    public Id SenderId { get; init; }
    public Id ReceiverId { get; init; }
    public InvitationStatus Status { get; private set; }
    public DateTime SentAt { get; init; }

    public void SetRejected()
    {
        Status = InvitationStatus.Rejected;
    }

    public void SetAccepted()
    {
        RaiseDomainEvent(new AcceptedInvitationDomainEvent(
            SenderId.Value.ToString(),
            ReceiverId.Value.ToString()));
        
        Status = InvitationStatus.Accepted;
    } 

    public bool IsHandled()
        => Status.Name == InvitationStatus.Accepted.Name ||
           Status.Name == InvitationStatus.Rejected.Name;

    public FriendInvitationSnapshot Save()
    {
        return new FriendInvitationSnapshot()
        {
            Id = Id.Value.ToString(),
            ReceiverId = ReceiverId.Value.ToString(),
            SenderId = SenderId.Value.ToString(),
            InvitationStatus = Status.Name,
            SentAtUtc = SentAt.ToUniversalTime(),
        };
    }
    
    public static FriendInvitation RestoreFromSnapshot(FriendInvitationSnapshot snapshot)
    {
        var result = FriendInvitation.Create(
            snapshot.Id,
            snapshot.SenderId,
            snapshot.ReceiverId,
            snapshot.InvitationStatus,
            snapshot.SentAtUtc);

        if (result.IsFailure)
            return null!;
        return result.Value;
    }

    public static Id GetRandomId()
    {
        var id = Id.Create(Guid.NewGuid().ToString());
        return id.Value;
    }
    
    public static Result<FriendInvitation> Create(
        string id, string senderId, string receiverId, string status, DateTime sentAt)
    {
        if (senderId == receiverId)
            return Result.Failure<FriendInvitation>(InvitationErrors.CantSendInvitationToMyself);
        
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<FriendInvitation>(idResult.Error);

        var senderIdResult = Id.Create(senderId);
        if (senderIdResult.IsFailure)
            return Result.Failure<FriendInvitation>(senderIdResult.Error);
        
        var receiverIdResult = Id.Create(receiverId);
        if (receiverIdResult.IsFailure)
            return Result.Failure<FriendInvitation>(receiverIdResult.Error);

        var statusResult = InvitationStatus.FromName(status);
        if (statusResult is null)
            return Result.Failure<FriendInvitation>(InvitationErrors.InvalidInvitationStatus);

        if (sentAt > DateTime.Now)
            return Result.Failure<FriendInvitation>(InvitationErrors.InvalidSentTime);

        return new FriendInvitation(
            idResult.Value,
            senderIdResult.Value,
            receiverIdResult.Value,
            statusResult,
            sentAt);
    }
}