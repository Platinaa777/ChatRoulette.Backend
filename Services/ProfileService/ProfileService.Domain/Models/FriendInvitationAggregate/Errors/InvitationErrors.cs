using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.FriendInvitationAggregate.Errors;

public class InvitationErrors : Error
{
    public static readonly Error InvalidSentTime = new("Invitation.Error", "Time cant be greater than current time");
    public static readonly Error InvalidInvitationStatus = new("Invitation.Error", "Current status does not exists");
    public static readonly Error InvitationAlreadyExist = new("Invitation.Error", "Invitation between these users is already exist");
    public static readonly Error CantAddInvitation = new("Invitation.Error", "Invitation cant be added");
    public static readonly Error CantSendInvitationToMyself = new("Invitation.Error", "User cant send invitation to myself");
    public static readonly Error InvitationDoesNotExist = new("Invitation.Error", "Invitation between users does not exist");
    public static readonly Error CantUpdateInvitationStatus = new("Invitation.Error", "Invitation cant be updated");
    public static readonly Error AlreadyFriends = new("Invitation.Error", "Already friends with this user");
    public static readonly Error InvalidOperation = new("Invitation.Error", "Cant update invitation status because is already handled");
    public static readonly Error DeletingError = new("Invitation.Error", "Cant close request about request invitation");
    public static readonly Error NoInvitations = new("Invitation.Error", "This user does not have any invitations to friend list");

    
    public InvitationErrors(string code, string message) : base(code, message)
    {
    }
}