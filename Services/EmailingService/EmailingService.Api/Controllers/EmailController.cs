using EmailingService.Api.Configuration;
using EmailingService.Api.Consumers;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailingService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    
    private readonly SmtpClientConfig _emailConfiguration;

    public EmailController(SmtpClientConfig emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailConfiguration.Email));
        email.To.Add(MailboxAddress.Parse(_emailConfiguration.Email));
        email.Subject = "Confirmation Email";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = "Confirm email"
        };

        using var smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(_emailConfiguration.Email, _emailConfiguration.Password);
        smtpClient.Send(email);
        smtpClient.Disconnect(true);
        
        return Ok(Task.FromResult("hello"));
    } 
}