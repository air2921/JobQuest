using Amazon.S3;

namespace infrastructure.S3;

public class S3ClientObject
{
    public string Bucket { get; set; } = null!;
    public AmazonS3Client S3Client { get; set; } = null!;
}
