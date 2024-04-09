using Emailing.HttpModels.Requests;
using EmailingService.Api.Configuration;
using EmailingService.Api.EmailUtils;
using EmailingService.Utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit.Client.EventBus;
using MassTransit.Contracts.UserEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit;

namespace EmailingService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly IEventBusClient _bus;
    private readonly SmtpClientConfig _smtpClient;

    public EmailController(
        IDistributedCache cache,
        IEventBusClient bus,
        SmtpClientConfig smtpClient)
    {
        _cache = cache;
        _bus = bus;
        _smtpClient = smtpClient;
    }

    [HttpGet("/confirm/{code}")]
    public async Task<ActionResult<bool>> ActivateAccount([FromRoute] string code, CancellationToken token = default)
    {
        var storedEmail = await _cache.GetStringAsync(code);

        if (storedEmail == null)
            return NotFound();

        await _bus.PublishAsync(new UserSubmittedEmail(storedEmail), token);
        
        return Redirect("http://localhost:3000");
    }

    [HttpGet]
    public string Test()
    {
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse("langskillup@gmail.com"));
        email.To.Add(MailboxAddress.Parse("miroshnichenkodenis2004@gmail.com"));
        email.Subject = "Confirmation Email";
        
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html);

        string code = Guid.NewGuid().ToString();
        
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = EmailTemplate.START + EmailApi.Url + code + EmailTemplate.END
        };
        
        using var smtpClient = new SmtpClient();
        smtpClient.Connect(_smtpClient.SmtpServer, _smtpClient.Port, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(_smtpClient.Email, _smtpClient.Password);
        smtpClient.Send(email);
        smtpClient.Disconnect(true);

        return "yes";
    }
}