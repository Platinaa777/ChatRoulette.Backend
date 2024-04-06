using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.Commands.ChangeAvatar;
using ProfileService.Application.Commands.GenerateNewAvatarUrl;
using ProfileService.Application.Models;
using S3.Client;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AvatarController : ControllerBase
{
    private readonly IS3Client _s3Client;
    private readonly IMediator _mediator;

    public AvatarController(
        IS3Client s3Client,
        IMediator mediator)
    {
        _s3Client = s3Client;
        _mediator = mediator;
    }

    [HttpPost("change-avatar/{email}")]
    public async Task<ActionResult<Result<AvatarInformation>>> ChangeAvatar(IFormFile formFile, [FromRoute] string email)
    {
        var result = await _mediator.Send(new ChangeAvatarCommand()
        {
            Avatar = formFile.OpenReadStream(),
            ContentType = formFile.ContentType,
            Email = email
        });

        if (result.IsFailure)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpGet("get-buckets")]
    public async Task<List<string>> GetBuckets()
    {
        return await _s3Client.GetBuckets();
    }
    
    [HttpGet("create-bucket")]
    public async Task<ActionResult> CreateBucket(string bucket)
    {
        await _s3Client.CreateBucket(bucket);
        return Ok();
    }

    [HttpPost("{email}")]
    public async Task<ActionResult> GenerateNewAvatarUrl(string email)
    {
        var result = await _mediator.Send(new GenerateNewAvatarUrlCommand()
        {
            Email = email
        });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
}