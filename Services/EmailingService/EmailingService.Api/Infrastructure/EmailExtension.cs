using EmailingService.Api.Configuration;
using EmailingService.Api.EmailUtils;
using Microsoft.Extensions.Options;

namespace EmailingService.Api.Infrastructure;

public static class EmailExtension
{
    public static WebApplicationBuilder AddEmailConfig(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

        builder.Services.Configure<SmtpClientConfig>(configuration.GetSection("SmtpClientConfig"));
        builder.Services.AddSingleton<SmtpClientConfig>(sp =>
        {
            var smtpClientConfig = sp.GetRequiredService<IOptions<SmtpClientConfig>>().Value;
            
            return new SmtpClientConfig()
            {
                Email = smtpClientConfig.Email,
                Password = password!,
                Port = smtpClientConfig.Port,
                SmtpServer = smtpClientConfig.SmtpServer,
                UserName = smtpClientConfig.UserName
            };
        });

        builder.Services.Configure<RedirectUrl>(configuration.GetSection("RedirectUrl"));
        builder.Services.AddSingleton<RedirectUrl>(sp =>
            sp.GetRequiredService<IOptions<RedirectUrl>>().Value);
        
        return builder;
    }    
}