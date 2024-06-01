using Amazon.S3;
using infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;

namespace infrastructure.S3
{
    public class S3ClientProvider(IConfiguration configuration) : IS3ClientProvider
    {
        public S3ClientObject GetS3Client()
        {
            var keyId = configuration.GetSection("S3")["keyId"]!;
            var accessKey = configuration.GetSection("S3")["accessKey"]!;
            var bucket = configuration.GetSection("S3")["bucket"]!;

            return new S3ClientObject
            {
                Bucket = bucket,
                S3Client = new AmazonS3Client(keyId, accessKey, new AmazonS3Config
                {
                    ServiceURL = "https://s3.yandexcloud.net"
                })
            };
        }
    }
}
