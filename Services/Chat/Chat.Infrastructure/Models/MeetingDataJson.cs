using Chat.Infrastructure.Communication;
using Newtonsoft.Json;

namespace Chat.Infrastructure.Models;

public class MeetingDataJson
{
    [JsonProperty("meetings")]
    public List<MeetingJson> Meetings { get; set; }
}