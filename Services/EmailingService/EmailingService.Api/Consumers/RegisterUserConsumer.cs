using EmailingService.Api.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MassTransit.Contracts.UserEvents;
using MimeKit;

namespace EmailingService.Api.Consumers;

public class RegisterUserConsumer : IConsumer<UserRegistered>
{
    private readonly SmtpClientConfig _emailConfiguration;
    private readonly ILogger<RegisterUserConsumer> _logger;

    public RegisterUserConsumer(SmtpClientConfig emailConfiguration, ILogger<RegisterUserConsumer> logger)
    {
        _emailConfiguration = emailConfiguration;
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<UserRegistered> context)
    {
        _logger.LogInformation($"Consumed message : {context.Message.Email}");
        
        
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
        
        return Task.CompletedTask;
    }
}