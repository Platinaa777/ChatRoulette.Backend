using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Domain.SeedWork;

namespace ProfileService.Domain.Models.UserProfileAggregate.Entities;

public class UserProfile : Entity<string>
{
    public Name UserName { get; set; }
    public Name NickName { get; set; }
    public Email Email { get; set; }
    public List<Preference> Preferences { get; set; }
}