namespace Emailing.HttpModels.Requests;

public class ConfirmEmailRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}