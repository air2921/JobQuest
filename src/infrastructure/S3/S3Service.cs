﻿using Amazon.S3.Model;
using domain.Abstractions;
using common.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace infrastructure.S3;

public class S3Service(S3ClientProvider s3provider, ILogger<S3Service> logger) : IS3Service
{
    private readonly object _lockObj = new();

    private readonly S3ClientObject _provider = s3provider.GetS3Client();
    private const string ERROR_GET = "Неизвестная ошибка при получении данных";
    private const string ERROR_POST = "Неизвестная ошибка при загрузке данных";
    private const string ERROR_DELETE = "Неизвестная ошибка при удалении данных";

    public async Task Upload(Stream stream, string key, CancellationToken token = default)
    {
        try
        {
            await _provider.S3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _provider.Bucket,
                Key = key,
                InputStream = stream
            }, token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, key);
            throw new S3Exception(ERROR_POST);
        }
    }

    public async Task<Stream> Download(string key, CancellationToken token = default)
    {
        try
        {
            var response = await _provider.S3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _provider.Bucket,
                Key = key
            }, token);

            return response.ResponseStream;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, key);
            throw new S3Exception(ERROR_GET);
        }
    }

    public async Task<Dictionary<string, Stream>> DownloadCollection(IEnumerable<string> keys, CancellationToken token = default)
    {
        var fileStreams = new Dictionary<string, Stream>();

        var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        var linkedToken = cts.Token;

        try
        {
            var tasks = keys.Select(async key =>
            {
                var stream = await Download(key, linkedToken);
                lock (_lockObj)
                {
                    fileStreams.Add(key, stream);
                }
            });

            await Task.WhenAll(tasks);
        }
        catch (Exception)
        {
            cts.Cancel();
            foreach (var stream in fileStreams.Values)
                stream?.Dispose();

            fileStreams.Clear();
            throw;
        }

        return fileStreams;
    }

    public async Task Delete(string key, CancellationToken token = default)
    {
        try
        {
            await _provider.S3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _provider.Bucket,
                Key = key
            }, token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, key);
            throw new S3Exception(ERROR_DELETE);
        }
    }
}
