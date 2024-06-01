using infrastructure.S3;

namespace infrastructure.Abstractions
{
    public interface IS3ClientProvider
    {
        S3ClientObject GetS3Client();
    }
}
