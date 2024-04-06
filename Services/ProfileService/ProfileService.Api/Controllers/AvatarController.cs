using Microsoft.AspNetCore.Mvc;
using S3.Client;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AvatarController : ControllerBase
{
    private readonly IS3Client _s3Client;

    public AvatarController(IS3Client s3Client)
    {
        _s3Client = s3Client;
    }

    [HttpPost("change-avatar/{email}")]
    public async Task<ActionResult> Get([FromBody] IFormFile formFile, [FromQuery] string email)
    {
        
        return Ok(formFile.ContentType);
    }

    [HttpGet("get-buckets")]
    public async Task<List<string>> GetBuckets()
    {
        return await _s3Client.GetBuckets();
    }
}