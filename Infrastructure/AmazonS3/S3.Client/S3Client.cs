using Amazon.S3;
using Amazon.S3.Model;
using S3.Contracts;

namespace S3.Client;

public class S3Client : IS3Client
{
    private readonly IAmazonS3 _amazonS3;

    public S3Client(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }
    
    public async Task<S3ObjectDto?> UploadFileAsync(string avatar, string bucket, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = bucket,
            Key = filename,
            ContentBody = avatar
        };
        
        // request.Metadata.Add("Content-Type", contentType);
        await _amazonS3.PutObjectAsync(request);

        return await FindFileAsync(bucket, filename);
    }

    public async Task<S3ObjectDto?> FindFileAsync(string bucket, string filename)
    {
        try
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = filename
            };
        
            GetObjectResponse response = await _amazonS3.GetObjectAsync(request);

            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucket,
                Key = filename,
                Expires = DateTime.Now.AddHours(180)
            };

            S3ObjectDto s3Object = new S3ObjectDto()
            {
                Name = response.Key,
                Link = await _amazonS3.GetPreSignedURLAsync(urlRequest)
            };
            
            return s3Object;
        }
        catch (Exception e)
        {
            Console.WriteLine($"bucket: {bucket}, filename: {filename} not found");
        }

        return null;
    }

    public async Task<List<string>> GetBuckets()
    {
        List<string> list = new();
        var buckets = await _amazonS3.ListBucketsAsync();
        foreach (var bucket in buckets.Buckets)
        {
            list.Add(bucket.BucketName);
        }

        return list;
    }

    public async Task DeleteFile(string bucket, string key)
    {
        await _amazonS3.DeleteObjectAsync(bucket, key);
    }

    public async Task CreateBucket(string bucket)
    {
        await _amazonS3.PutBucketAsync(bucket);
    }
}