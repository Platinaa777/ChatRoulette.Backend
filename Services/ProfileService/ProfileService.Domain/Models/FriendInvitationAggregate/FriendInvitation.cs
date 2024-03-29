using ProfileService.Domain.Models.FriendInvitationAggregate.Enumerations;
using ProfileService.Domain.Models.FriendInvitationAggregate.Errors;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.FriendInvitationAggregate;

public class FriendInvitation : AggregateRoot<Id>
{
    private FriendInvitation(
        Id id,
        Email senderEmail,
        Email receiverEmail,
        InvitationStatus status,
        DateTime sentAt)
    {
        Id = id;
        SenderEmail = senderEmail;
        ReceiverEmail = receiverEmail;
        Status = status;
        SentAt = sentAt;
    }
    
    public Email SenderEmail { get; init; }
    public Email ReceiverEmail { get; init; }
    public InvitationStatus Status { get; set; }
    public DateTime SentAt { get; init; }
    
    public static Result<FriendInvitation> Create(
        string id, string senderEmail, string receiverEmail, string status, DateTime sentAt)
    {
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<FriendInvitation>(idResult.Error);

        var senderEmailResult = Email.Create(senderEmail);
        if (senderEmailResult.IsFailure)
            return Result.Failure<FriendInvitation>(senderEmailResult.Error);
        
        var receiverEmailResult = Email.Create(receiverEmail);
        if (receiverEmailResult.IsFailure)
            return Result.Failure<FriendInvitation>(receiverEmailResult.Error);

        var statusResult = InvitationStatus.FromName(status);
        if (statusResult is null)
            return Result.Failure<FriendInvitation>(InvitationErrors.InvalidInvitationStatus);

        if (sentAt > DateTime.Now)
            return Result.Failure<FriendInvitation>(InvitationErrors.InvalidSentTime);

        return new FriendInvitation(
            idResult.Value,
            senderEmailResult.Value,
            receiverEmailResult.Value,
            statusResult,
            sentAt);
    }
}