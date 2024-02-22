using System.ComponentModel.DataAnnotations;

namespace AuthService.HttpModels.Requests;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string NickName { get; set; }
    [Required]
    public int? Age { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required] 
    public string[] Preferences { get; set; }
}