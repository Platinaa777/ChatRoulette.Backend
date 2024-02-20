using EmailingService.Api.Configuration;
using Microsoft.Extensions.Options;

namespace EmailingService.Api.Infrastructure;

public static class EmailExtension
{
    public static WebApplicationBuilder AddEmailConfig(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.Configure<SmtpClientConfig>(configuration.GetSection("SmtpClientConfig"));
        builder.Services.AddSingleton<SmtpClientConfig>(sp =>
            sp.GetRequiredService<IOptions<SmtpClientConfig>>().Value);

        return builder;
    }    
}