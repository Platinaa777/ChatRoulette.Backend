using System.ComponentModel.DataAnnotations;

namespace AuthService.HttpModels.Requests;

public class TokenRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}