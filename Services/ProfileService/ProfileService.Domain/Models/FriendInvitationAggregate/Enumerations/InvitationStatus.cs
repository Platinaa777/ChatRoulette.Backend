using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.FriendInvitationAggregate.Enumerations;

public class InvitationStatus : Enumeration
{
    public static readonly InvitationStatus Pending = new InvitationStatus(1, nameof(Pending));
    public static readonly InvitationStatus Accepted = new InvitationStatus(1, nameof(Accepted));
    public static readonly InvitationStatus Rejected = new InvitationStatus(1, nameof(Rejected));
    
    public InvitationStatus(int id, string name) : base(id, name)
    {
    }
    
    public static InvitationStatus? FromName(string status)
    {
        var collection = GetAll<InvitationStatus>();
        foreach (var invitationStatus in collection)
        {
            if (invitationStatus.Name == status)
                return invitationStatus;
        }

        return null;
    }
    
    public static InvitationStatus? FromValue(int id)
    {
        var collection = GetAll<InvitationStatus>();
        foreach (var invitationStatus in collection)
        {
            if (invitationStatus.Id == id)
                return invitationStatus;
        }

        return null;
    }
}