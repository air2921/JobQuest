using infrastructure.S3;

namespace infrastructure.Abstractions
{
    public interface IS3ClientProvider
    {
        public S3ClientObject GetS3Client();
    }
}
