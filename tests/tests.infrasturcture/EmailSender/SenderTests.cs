using common;
using common.DTO;
using domain.Abstractions;
using infrastructure.Abstractions;
using infrastructure.EmailSender;
using JsonLocalizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using common.Exceptions;
using Moq;

namespace tests.infrasturcture.EmailSender;

public class SenderTests
{
    private const string USERNAME = "testuser";
    private const string EMAIL = "testuser@example.com";
    private const string SUBJECT = "testSubject";
    private const string BODY = "testMessage";
    private const string EMAIL_FROM = "emailFrom";
    private const string NAME_FROM = "JobQuest";

    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ISmtpClientWrapper> _mockSmtpClientWrapper;
    private readonly Mock<ILocalizer> _mockLocalizer;
    private readonly Mock<ILogger<Sender>> _mockLogger;

    private readonly Mock<ISender<EmailDTO>> _mockSender;

    private readonly EmailDTO _emailDTO = new()
    {
        Username = USERNAME,
        Email = EMAIL,
        Subject = SUBJECT,
        Body = BODY
    };

    public SenderTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockSmtpClientWrapper = new Mock<ISmtpClientWrapper>();
        _mockLocalizer = new Mock<ILocalizer>();
        _mockLogger = new Mock<ILogger<Sender>>();
        _mockSender = new Mock<ISender<EmailDTO>>();
      
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(s => s[App.EMAIL]).Returns(EMAIL_FROM);
        _mockConfiguration.Setup(c => c.GetSection(App.EMAIL_SECTION)).Returns(mockSection.Object);
    }

    [Fact]
    public async Task Send_Success()
    {
        var sender = new Sender(_mockConfiguration.Object, _mockLogger.Object,
            _mockSmtpClientWrapper.Object, _mockLocalizer.Object);

        await sender.SendMessage(_emailDTO);

        _mockSmtpClientWrapper.Verify(e => e.EmailSendAsync(
            It.Is<MimeMessage>(msg =>
                msg.From.Mailboxes.Single().Name == NAME_FROM &&
                msg.From.Mailboxes.Single().Address == EMAIL_FROM &&
                msg.To.Mailboxes.Single().Name == USERNAME &&
                msg.To.Mailboxes.Single().Address == EMAIL &&
                msg.Subject == SUBJECT &&
                ((TextPart)msg.Body).Text == BODY), It.IsAny<CancellationToken>()),
        Times.Once);
    }

    [Theory]
    [InlineData(typeof(SmtpClientException))]
    [InlineData(typeof(Exception))]
    public async Task Send_ThrowException(Type exceptionType)
    {
        var exception = (Exception)Activator.CreateInstance(exceptionType, "")!;
        var sender = new Sender(_mockConfiguration.Object, _mockLogger.Object,
            _mockSmtpClientWrapper.Object, _mockLocalizer.Object);

        _mockSmtpClientWrapper.Setup(e => e.EmailSendAsync(
            It.Is<MimeMessage>(msg =>
                msg.From.Mailboxes.Single().Name == NAME_FROM &&
                msg.From.Mailboxes.Single().Address == EMAIL_FROM &&
                msg.To.Mailboxes.Single().Name == USERNAME &&
                msg.To.Mailboxes.Single().Address == EMAIL &&
                msg.Subject == SUBJECT &&
                ((TextPart)msg.Body).Text == BODY), It.IsAny<CancellationToken>()))
            .Throws(exception);

        await Assert.ThrowsAsync<SmtpClientException>(() => sender.SendMessage(_emailDTO));
    }

    private MimeMessage SetMessage()
    {
        MimeMessage emailMessage = new();
        emailMessage.From.Add(new MailboxAddress(NAME_FROM, EMAIL_FROM));
        emailMessage.To.Add(new MailboxAddress(USERNAME, EMAIL));
        emailMessage.Subject = SUBJECT;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = BODY
        };

        return emailMessage;
    }
}
