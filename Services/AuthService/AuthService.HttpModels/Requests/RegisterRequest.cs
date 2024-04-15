using System.ComponentModel.DataAnnotations;

namespace AuthService.HttpModels.Requests;

public class RegisterRequest
{
    public string UserName { get; set; }
    public DateTime BirthDateUtc { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}