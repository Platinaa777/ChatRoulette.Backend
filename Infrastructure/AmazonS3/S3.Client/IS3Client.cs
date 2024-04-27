using S3.Contracts;

namespace S3.Client;

public interface IS3Client
{
    public Task<S3ObjectDto?> UploadFileAsync(Stream avatar, string bucket, string filename, string contentType);
    public Task<S3ObjectDto?> FindFileAsync(string bucket, string filename);
    public Task<List<string>> GetBuckets();
    public Task DeleteFile(string bucket, string key);
    public Task CreateBucket(string bucket);
} 