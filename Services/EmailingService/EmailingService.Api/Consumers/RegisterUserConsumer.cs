using EmailingService.Api.Configuration;
using EmailingService.Utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MassTransit.Contracts.UserEvents;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit;

namespace EmailingService.Api.Consumers;

public class RegisterUserConsumer : IConsumer<UserRegistered>
{
    private readonly SmtpClientConfig _emailConfiguration;
    private readonly ILogger<RegisterUserConsumer> _logger;
    private readonly IDistributedCache _cache;

    public RegisterUserConsumer(
        SmtpClientConfig emailConfiguration,
        ILogger<RegisterUserConsumer> logger,
        IDistributedCache cache)
    {
        _emailConfiguration = emailConfiguration;
        _logger = logger;
        _cache = cache;
    }
    
    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        _logger.LogInformation($"Consumed message : email={context.Message.Email}; username={context.Message.UserName}");
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailConfiguration.Email));
        email.To.Add(MailboxAddress.Parse(context.Message.Email));
        email.Subject = "Confirmation Email";
        
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html);

        string code = Guid.NewGuid().ToString();
        
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"Confirm email: Please link to this {EmailApi.Url + code}"
        };
        await _cache.SetStringAsync(code,
            context.Message.Email,
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            },
            context.CancellationToken);

        using var smtpClient = new SmtpClient();
        smtpClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(_emailConfiguration.Email, _emailConfiguration.Password);
        smtpClient.Send(email);
        smtpClient.Disconnect(true);
    }
}