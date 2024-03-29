using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.FriendInvitationAggregate.Errors;

public class InvitationErrors : Error
{
    public static readonly Error InvalidSentTime = new("Invitation.Error", "Time cant be greater than current time");
    public static readonly Error InvalidInvitationStatus = new("Invitation.Error", "Current status does not exists");

    
    public InvitationErrors(string code, string message) : base(code, message)
    {
    }
}