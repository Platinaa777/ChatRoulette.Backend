using EmailingService.Api.Configuration;
using EmailingService.Api.EmailUtils;
using Microsoft.Extensions.Options;

namespace EmailingService.Api.Infrastructure;

public static class EmailExtension
{
    public static WebApplicationBuilder AddEmailConfig(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.Configure<SmtpClientConfig>(configuration.GetSection("SmtpClientConfig"));
        builder.Services.AddSingleton<SmtpClientConfig>(sp =>
        {
            var smtpClientConfig = sp.GetRequiredService<IOptions<SmtpClientConfig>>().Value;

            Console.WriteLine(smtpClientConfig.Password[0]);
            return new SmtpClientConfig()
            {
                Email = smtpClientConfig.Email,
                Password = smtpClientConfig.Password,
                Port = smtpClientConfig.Port,
                SmtpServer = smtpClientConfig.SmtpServer,
                UserName = smtpClientConfig.UserName
            };
        });

        builder.Services.Configure<RedirectUrl>(configuration.GetSection("RedirectUrl"));
        builder.Services.AddSingleton<RedirectUrl>(sp =>
            sp.GetRequiredService<IOptions<RedirectUrl>>().Value);
        
        builder.Services.Configure<ApiUrl>(configuration.GetSection("ApiUrl"));
        builder.Services.AddSingleton<ApiUrl>(sp =>
            sp.GetRequiredService<IOptions<ApiUrl>>().Value);
        
        return builder;
    }    
}