using Amazon.S3.Model;
using infrastructure.S3;
using JsonLocalizer;
using Microsoft.Extensions.Logging;
using Moq;
using Amazon.S3;
using domain.Abstractions;
using common.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;

namespace tests.infrasturcture.S3;

public class S3ServiceTests
{
    private readonly Mock<IS3ClientProvider> _mockS3ClientProvider;
    private readonly Mock<ILogger<S3Service>> _mockLogger;
    private readonly Mock<ILocalizer> _mockLocalizer;
    private readonly IS3Service _s3Service;
    private readonly Mock<IAmazonS3> _mockS3Client;

    public S3ServiceTests()
    {
        _mockS3ClientProvider = new Mock<IS3ClientProvider>();
        _mockLogger = new Mock<ILogger<S3Service>>();
        _mockLocalizer = new Mock<ILocalizer>();
        _mockS3Client = new Mock<IAmazonS3>();

        _mockS3ClientProvider
            .Setup(p => p.GetS3Client())
            .Returns(new S3ClientObject
            {
                S3Client = _mockS3Client.Object,
                Bucket = "test-bucket"
            });

        _s3Service = new S3Service(_mockS3ClientProvider.Object, _mockLogger.Object, _mockLocalizer.Object);
    }

    [Fact]
    public async Task Upload_ShouldCallPutObjectAsync_WhenCalled()
    {
        var stream = new MemoryStream();
        var key = "test-key";
        var provider = _mockS3ClientProvider.Object.GetS3Client();

        await _s3Service.Upload(stream, key);

        _mockS3Client
            .Verify(s3 => s3.PutObjectAsync(It.Is<PutObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == key &&
                req.InputStream == stream),
                CancellationToken.None),
                Times.Once);
    }

    [Fact]
    public async Task Download_ShouldCallGetObjectAsync_WhenCalled()
    {
        var key = "test-key";
        var mockResponseStream = CreateRandomStream();
        var mockResponseStreamCopy = new MemoryStream(mockResponseStream.ToArray());
        var provider = _mockS3ClientProvider.Object.GetS3Client();

        _mockS3Client
            .Setup(s3 => s3.GetObjectAsync(It.Is<GetObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == key), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse { ResponseStream = mockResponseStream });

        var result = await _s3Service.Download(key);
        Assert.True(AreStreamsEqual(result, mockResponseStreamCopy));
    }

    [Fact]
    public async Task DownloadCollection_ShouldCallGetObjectAsync_WhenCalled()
    {
        var provider = _mockS3ClientProvider.Object.GetS3Client();

        var stream1 = CreateRandomStream();
        var stream1Copy = new MemoryStream(stream1.ToArray());
        var stream2 = CreateRandomStream();
        var stream2Copy = new MemoryStream(stream2.ToArray());
        string[] keys = ["key1", "key2"];
        var dictionary = new Dictionary<string, Stream>
        {
            { "key1", stream1 },
            { "key2", stream2 }
        };

        _mockS3Client
            .Setup(s3 => s3.GetObjectAsync(It.Is<GetObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == "key1"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse { ResponseStream = stream1 });

        _mockS3Client
            .Setup(s3 => s3.GetObjectAsync(It.Is<GetObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == "key2"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse { ResponseStream = stream2 });


        var result = await _s3Service.DownloadCollection(keys);

        Assert.True(result.TryGetValue("key1", out var obj1));
        Assert.True(result.TryGetValue("key2", out var obj2));
        Assert.True(AreStreamsEqual(obj1, stream1Copy));
        Assert.True(AreStreamsEqual(obj2, stream2Copy));
    }

    [Fact]
    public async Task Delete_ShouldCallPutObjectAsync_WhenCalled()
    {
        var key = "test-key";
        var provider = _mockS3ClientProvider.Object.GetS3Client();

        await _s3Service.Delete(key);

        _mockS3Client
            .Verify(s3 => s3.DeleteObjectAsync(It.Is<DeleteObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == key),
                CancellationToken.None),
                Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldThrowS3Exception_WhenExceptionOccurs()
    {
        var key = "test-key";
        var s3Exception = new Exception("Some error");
        var provider = _mockS3ClientProvider.Object.GetS3Client();

        _mockS3Client
            .Setup(s3 => s3.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), CancellationToken.None))
            .ThrowsAsync(s3Exception);

        await Assert.ThrowsAsync<S3Exception>(() => _s3Service.Delete(key));
    }

    [Fact]
    public async Task Upload_ShouldThrowS3Exception_WhenExceptionOccurs()
    {
        var stream = new MemoryStream();
        var key = "test-key";
        var s3Exception = new Exception("Some error");

        _mockS3Client
            .Setup(s3 => s3.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(s3Exception);

        await Assert.ThrowsAsync<S3Exception>(() => _s3Service.Upload(stream, key));
    }

    [Fact]
    public async Task Download_ShouldThrowS3Exception_WhenExceptionOccurs()
    {
        var key = "test-key";
        var s3Exception = new Exception("Some error");

        _mockS3Client
            .Setup(s3 => s3.GetObjectAsync(It.IsAny<GetObjectRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(s3Exception);

        await Assert.ThrowsAsync<S3Exception>(() => _s3Service.Download(key));
    }

    [Fact]
    public async Task DownloadCollection_ShouldThrowS3Exception_WhenExceptionOccurs()
    {
        var provider = _mockS3ClientProvider.Object.GetS3Client();
        var s3Exception = new S3Exception("Some error");

        var stream1 = CreateRandomStream();
        var stream1Copy = new MemoryStream(stream1.ToArray());
        var stream2 = CreateRandomStream();
        var stream2Copy = new MemoryStream(stream2.ToArray());
        string[] keys = ["key1", "key2"];
        var dictionary = new Dictionary<string, Stream>
        {
            { "key1", stream1 },
            { "key2", stream2 }
        };

        _mockS3Client
            .Setup(s3 => s3.GetObjectAsync(It.Is<GetObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == "key1"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse { ResponseStream = stream1 });

        _mockS3Client
            .Setup(s3 => s3.GetObjectAsync(It.Is<GetObjectRequest>(req =>
                req.BucketName == provider.Bucket &&
                req.Key == "key2"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(s3Exception);

        await Assert.ThrowsAsync<S3Exception>(() => _s3Service.DownloadCollection(keys));
    }


    public static MemoryStream CreateRandomStream()
    {
        var random = new Random();

        byte[] buffer = new byte[128];
        random.NextBytes(buffer);

        var stream = new MemoryStream(buffer);
        return stream;
    }

    private bool AreStreamsEqual(Stream stream1, Stream stream2)
    {
        if (stream1.Length != stream2.Length)
        {
            return false;
        }

        stream1.Position = 0;
        stream2.Position = 0;

        using var reader1 = new BinaryReader(stream1);
        using var reader2 = new BinaryReader(stream2);

        byte[] bytes1 = reader1.ReadBytes((int)stream1.Length);
        byte[] bytes2 = reader2.ReadBytes((int)stream2.Length);

        return bytes1.SequenceEqual(bytes2);
    }
}
