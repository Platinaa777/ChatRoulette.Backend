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
    private readonly RedirectUrl _redirectUrl;

    public EmailController(
        IDistributedCache cache,
        IEventBusClient bus,
        RedirectUrl redirectUrl)
    {
        _cache = cache;
        _bus = bus;
        _redirectUrl = redirectUrl;
    }

    [HttpPost("/confirm/{code}")]
    public async Task<ActionResult<bool>> ActivateAccount(
        [FromRoute] string code,
        CancellationToken token = default)
    {
        var storedEmail = await _cache.GetStringAsync(code, token);

        if (storedEmail == null)
            return NotFound();

        await _bus.PublishAsync(new UserSubmittedEmail(storedEmail), token);
        
        return Redirect(_redirectUrl.Url);
    }
}