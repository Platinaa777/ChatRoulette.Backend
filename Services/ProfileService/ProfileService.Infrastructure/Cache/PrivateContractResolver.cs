using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProfileService.Infrastructure.Cache;

public class PrivateContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (!property.Writable)
        {
            var prop = member as PropertyInfo;

            property.Writable = prop?.GetSetMethod(true) != null;
        }

        return property;
    }
}