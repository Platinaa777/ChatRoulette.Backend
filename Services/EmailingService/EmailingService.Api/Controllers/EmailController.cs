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
    private readonly IDistributedCache _cache;
    private readonly IEventBusClient _bus;

    public EmailController(
        IDistributedCache cache,
        IEventBusClient bus)
    {
        _cache = cache;
        _bus = bus;
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
}