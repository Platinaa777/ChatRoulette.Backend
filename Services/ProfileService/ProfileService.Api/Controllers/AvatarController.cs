using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Models;
using ProfileService.Api.Utils;
using ProfileService.Application.Commands.ChangeAvatar;
using ProfileService.Application.Commands.GenerateNewAvatarUrl;
using ProfileService.Application.Constants;
using ProfileService.Application.Models;
using S3.Client;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("avatar")]
public class AvatarController : ControllerBase
{
    private readonly IS3Client _s3Client;
    private readonly IMediator _mediator;
    private readonly CredentialsChecker _credentialsChecker;

    public AvatarController(
        IS3Client s3Client,
        IMediator mediator,
        CredentialsChecker credentialsChecker)
    {
        _s3Client = s3Client;
        _mediator = mediator;
        _credentialsChecker = credentialsChecker;
    }

    [HttpPost("change-avatar")]
    public async Task<ActionResult<Result<AvatarInformation>>> ChangeAvatar([FromForm] AvatarRequest formFile)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var result = await _mediator.Send(new ChangeAvatarCommand()
        {
            Avatar = formFile.File.OpenReadStream(),
            ContentType = formFile.File.ContentType,
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

    [HttpGet("get-objects/{id}")]
    public async Task<ActionResult<List<string>>> GetPhoto(string id)
    {
        var s3Object = await _s3Client.FindFileAsync(bucket: S3Buckets.Achievement,
            filename: id + ".jpg");

        if (s3Object is null)
            return BadRequest();

        return Ok(s3Object.Link);
    }
    
    [HttpGet("create-bucket")]
    public async Task<ActionResult> CreateBucket(string bucket)
    {
        await _s3Client.CreateBucket(bucket);
        return Ok();
    }

    [HttpPut("refresh-avatar")]
    public async Task<ActionResult> GenerateNewAvatarUrl()
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var result = await _mediator.Send(new GenerateNewAvatarUrlCommand()
        {
            Email = email
        });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
}