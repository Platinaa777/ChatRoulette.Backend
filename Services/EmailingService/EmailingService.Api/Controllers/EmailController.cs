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
[Route("email")]
public class EmailController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly IEventBusClient _bus;
    private readonly RedirectUrl _redirectUrl;
    private readonly ILogger<EmailController> _logger;

    public EmailController(
        IDistributedCache cache,
        IEventBusClient bus,
        RedirectUrl redirectUrl,
        ILogger<EmailController> logger)
    {
        _cache = cache;
        _bus = bus;
        _redirectUrl = redirectUrl;
        _logger = logger;
    }

    [HttpGet("confirm/{code}")]
    public async Task<ActionResult<bool>> ActivateAccount(
        [FromRoute] string code,
        CancellationToken token = default)
    {
        var storedEmail = await _cache.GetStringAsync(code, token);
        
        _logger.LogInformation("Get stored email {@Email} by code: {@Code}", 
            storedEmail, code);
        
        if (storedEmail == null)
            return NotFound();

        await _bus.PublishAsync(new UserSubmittedEmail(storedEmail), token);
        
        _logger.LogInformation("Redirect url {@Url}", 
            _redirectUrl.Url);
        
        return Redirect(_redirectUrl.Url);
    }
}