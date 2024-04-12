namespace Chat.Application.Models;

public class GameInformation
{
    public string Word { get; set; }
    public List<string> ListTranslates { get; set; }
    public string RoundId { get; set; }
}