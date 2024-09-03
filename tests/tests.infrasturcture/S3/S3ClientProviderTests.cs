using infrastructure.S3;
using Microsoft.Extensions.Configuration;
using Moq;
using common;

namespace tests.infrasturcture.S3;

public class S3ClientProviderTests
{
    private const string _expectedKeyId = "testKeyId";
    private const string _expectedAccessKey = "testAccessKey";
    private const string _expectedBucket = "testBucket";
    private const string _expectedUrl = "https://s3.yandexcloud.net/";

    [Fact]
    public void GetS3Client_ShouldReturnS3ClientObject_WithCorrectConfiguration()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();

        mockSection.Setup(s => s[App.S3_KEY_ID]).Returns(_expectedKeyId);
        mockSection.Setup(s => s[App.S3_ACCESS_KEY]).Returns(_expectedAccessKey);
        mockSection.Setup(s => s[App.S3_BUCKET]).Returns(_expectedBucket);
        mockSection.Setup(s => s[App.S3_URL]).Returns(_expectedUrl);
        mockConfiguration.Setup(c => c.GetSection(App.S3_SECTION)).Returns(mockSection.Object);

        var s3ClientProvider = new S3ClientProvider(mockConfiguration.Object);
        var s3ClientObject = s3ClientProvider.GetS3Client();

        Assert.Equal(_expectedBucket, s3ClientObject.Bucket);
        Assert.Equal(_expectedUrl, s3ClientObject.S3Client.Config.ServiceURL);
        Assert.NotNull(s3ClientObject.S3Client);
    }
}
