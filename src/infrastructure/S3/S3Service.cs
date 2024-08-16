using Amazon.S3.Model;
using domain.Abstractions;
using common.Exceptions;
using infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace infrastructure.S3;

public class S3Service(IS3ClientProvider s3provider, ILogger<S3Service> logger) : IS3Service
{
    private readonly S3ClientObject _provider = s3provider.GetS3Client();
    private const string ERROR = "Неизвестная ошибка при получении данных";

    public async Task Upload(Stream stream, string key)
    {
        try
        {
            await _provider.S3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _provider.Bucket,
                Key = key,
                InputStream = stream
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw new S3Exception(ERROR);
        }
    }

    public async Task<Stream> Download(string key)
    {
        try
        {
            var response = await _provider.S3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _provider.Bucket,
                Key = key
            });

            return response.ResponseStream;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw new S3Exception(ERROR);
        }
    }

    public async Task Delete(string key)
    {
        try
        {
            await _provider.S3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _provider.Bucket,
                Key = key
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw new S3Exception(ERROR);
        }
    }
}
