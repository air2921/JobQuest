using infrastructure.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Net.Sockets;
using System.Threading.Tasks;
using common.Exceptions;
using System;
using common;
using JsonLocalizer;
using domain.Localize;
using System.Threading;

namespace infrastructure.EmailSender;

public class SmtpClientWrapper(
    ILogger<SmtpClientWrapper> logger,
    IConfiguration configuration,
    ILocalizer localizer) : ISmtpClientWrapper
{
    private readonly SmtpClient _smtpClient = new();

    public async Task EmailSendAsync(MimeMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            await _smtpClient.ConnectAsync("smtp.yandex.ru", 587, SecureSocketOptions.Auto, cancellationToken);
            await _smtpClient.AuthenticateAsync(configuration[App.EMAIL], configuration[App.EMAIL_PASSWORD], cancellationToken);
            await _smtpClient.SendAsync(message, cancellationToken);
        }
        catch (Exception ex) when (ex is AuthenticationException || ex is SocketException)
        {
            logger.LogError(ex.ToString(), nameof(EmailSendAsync));
            throw new SmtpClientException(localizer.Translate(Messages.MAIL_ERROR));
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Email sending operation was cancelled");
        }
        finally
        {
            try
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Disconnect operation was cancelled");
            }
            finally
            {
                _smtpClient.Dispose();
            }
        }
    }
}
