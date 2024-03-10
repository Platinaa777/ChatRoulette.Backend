using System.ComponentModel.DataAnnotations;

namespace AuthService.HttpModels.Requests;

public class TokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}