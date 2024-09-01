using domain.Abstractions;
using common.Exceptions;
using common.DTO;
using common;
using infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;
using JsonLocalizer;
using domain.Localize;
using System.Threading;

namespace infrastructure.EmailSender;

public class Sender(
    IConfiguration configuration,
    ILogger<Sender> logger,
    ISmtpClientWrapper smtpClient,
    ILocalizer localizer) : ISender<EmailDTO>
{
    public async Task SendMessage(EmailDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("JobQuest", configuration.GetSection(App.EMAIL_SECTION)[App.EMAIL]));
            emailMessage.To.Add(new MailboxAddress(dto.Username, dto.Email));
            emailMessage.Subject = dto.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = dto.Body
            };

            await smtpClient.EmailSendAsync(emailMessage, cancellationToken);
        }
        catch (SmtpClientException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, dto.Email);
            throw new SmtpClientException(localizer.Translate(Messages.MAIL_ERROR));
        }
    }
}
