using Newtonsoft.Json;
using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;

namespace ProfileService.Infrastructure.Repos.Models;

public class UserDb
{
    public string Id { get; set; }
    public string NickName { get; set; } = "nick_name";
    public string Email { get; set; }
    public int Age { get; set; }
    public string Preferences { get; set; }
}

public static class UserDbExtension
{
    public static Preference[] GetDomainPreferenceList(this UserDb user)
    {
        List<Preference> preferences = new();

        var jsonPreferences = JsonConvert.DeserializeObject<List<string>>(user.Preferences);

        if (jsonPreferences == null)
            return null;
        
        foreach (var preference in jsonPreferences)
        {
            var p = Preference.FromName(preference);
            if (p != null)
            {
                preferences.Add(p);
            }
        }

        return preferences.ToArray();
    }
}