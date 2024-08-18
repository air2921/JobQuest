using Amazon.S3;
using common;
using infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;

namespace infrastructure.S3;

public class S3ClientProvider(IConfiguration configuration)
{
    public S3ClientObject GetS3Client()
    {
        var keyId = configuration.GetSection(App.S3_SECTION)[App.S3_KEY_ID]!;
        var accessKey = configuration.GetSection(App.S3_SECTION)[App.S3_ACCESS_KEY]!;
        var bucket = configuration.GetSection(App.S3_SECTION)[App.S3_BUCKET]!;

        return new S3ClientObject
        {
            Bucket = bucket,
            S3Client = new AmazonS3Client(keyId, accessKey, new AmazonS3Config
            {
                ServiceURL = App.S3_URL
            })
        };
    }
}
