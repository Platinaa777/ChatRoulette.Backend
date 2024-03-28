namespace ProfileService.HttpModels.Requests;

public class ChangeNicknameRequest
{
    public string Email { get; set; }
    public string NickName { get; set; }
}