using Newtonsoft.Json;
using WaitingRoom.Infrastructure.Models;

namespace Chat.Infrastructure.Models;

public class MeetingsData
{
    [JsonProperty("meetings")]
    public List<Meeting> Meetings { get; set; }
}