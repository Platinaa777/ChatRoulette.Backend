namespace EmailingService.Api.Configuration;

public class SmtpClientConfig
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
}