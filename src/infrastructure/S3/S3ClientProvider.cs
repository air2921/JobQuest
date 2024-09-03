using Amazon.S3;
using common;
using Microsoft.Extensions.Configuration;

namespace infrastructure.S3;

public class S3ClientProvider(IConfiguration configuration) : IS3ClientProvider
{
    public S3ClientObject GetS3Client()
    {
        var section = configuration.GetSection(App.S3_SECTION);

        var keyId = section[App.S3_KEY_ID]!;
        var accessKey = section[App.S3_ACCESS_KEY]!;
        var bucket = section[App.S3_BUCKET]!;

        return new S3ClientObject
        {
            Bucket = bucket,
            S3Client = new AmazonS3Client(keyId, accessKey, new AmazonS3Config
            {
                ServiceURL = section[App.S3_URL]
            })
        };
    }
}

public interface IS3ClientProvider
{
    S3ClientObject GetS3Client();
}
