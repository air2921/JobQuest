using Amazon.S3.Model;
using domain.Abstractions;
using common.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace infrastructure.S3;

public class S3Service(S3ClientProvider s3provider, ILogger<S3Service> logger) : IS3Service
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
            logger.LogError(ex.Message);
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
            logger.LogError(ex.Message, key);
            throw new S3Exception(ERROR);
        }
    }

    public async Task<Dictionary<string, Stream>> DownloadCollection(IEnumerable<string> keys)
    {
        var fileStreams = new Dictionary<string, Stream>();

        var tasks = keys.Select(async key =>
        {
            try
            {
                var stream = await Download(key);
                fileStreams.Add(key, stream);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, key);
            }
        });

        await Task.WhenAll(tasks);

        return fileStreams;
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
            logger.LogError(ex.Message);
            throw new S3Exception(ERROR);
        }
    }
}
