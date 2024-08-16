using infrastructure.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Net.Sockets;
using System.Threading.Tasks;
using common.Exceptions;

namespace infrastructure.EmailSender;

public class SmtpClientWrapper(
    ILogger<SmtpClientWrapper> logger,
    IConfiguration configuration) : ISmtpClientWrapper
{
    private readonly SmtpClient _smtpClient = new();

    public async Task EmailSendAsync(MimeMessage message)
    {
        try
        {
            await _smtpClient.ConnectAsync("smtp.yandex.ru", 587, SecureSocketOptions.Auto);
            await _smtpClient.AuthenticateAsync(configuration["Email"], configuration["EmailPassword"]);
            await _smtpClient.SendAsync(message);
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.ToString(), nameof(EmailSendAsync));
            throw new SmtpClientException("Error sending message");
        }
        catch (SocketException ex)
        {
            logger.LogError(ex.ToString(), nameof(EmailSendAsync));
            throw new SmtpClientException("Error sending message");
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true);
            _smtpClient.Dispose();
        }
    }
}
