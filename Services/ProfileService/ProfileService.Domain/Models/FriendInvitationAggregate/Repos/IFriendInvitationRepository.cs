using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Domain.Models.FriendInvitationAggregate.Repos;

public interface IFriendInvitationRepository
{
    Task<FriendInvitation?> FindByInvitationId(Id id);
    Task<FriendInvitation?> FindByProfileIds(Id senderId, Id receiverId);
    Task<bool> Add(FriendInvitation invitation);
    Task<bool> Update(FriendInvitation invitation);
    Task<bool> Remove(Id invitationId);
}