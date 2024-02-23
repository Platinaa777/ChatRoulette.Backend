using Emailing.HttpModels.Requests;
using EmailingService.Api.Configuration;
using MassTransit.Client.EventBus;
using MassTransit.Contracts.UserEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace EmailingService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly SmtpClientConfig _emailConfiguration;
    private readonly IDistributedCache _cache;
    private readonly IEventBusClient _bus;

    public EmailController(
        SmtpClientConfig emailConfiguration,
        IDistributedCache cache,
        IEventBusClient bus)
    {
        _emailConfiguration = emailConfiguration;
        _cache = cache;
        _bus = bus;
    }

    [HttpPost("/confirm/{code}")]
    public async Task<ActionResult<bool>> ActivateAccount([FromBody] ConfirmEmailRequest req, CancellationToken token = default)
    {
        var storedCode = await _cache.GetStringAsync(req.Email);

        if (storedCode == null)
            return NotFound();

        if (req.Code != storedCode)
            return BadRequest();
        
        await _bus.PublishAsync(new UserSubmittedEmail(req.Email), token);
        
        return Ok(true);
    }
}