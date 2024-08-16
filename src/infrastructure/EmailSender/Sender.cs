using domain.Abstractions;
using common.Exceptions;
using common.DTO;
using infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace infrastructure.EmailSender;

public class Sender(
    IConfiguration configuration,
    ILogger<Sender> logger,
    ISmtpClientWrapper smtpClient) : ISender<EmailDTO>
{
    public async Task SendMessage(EmailDTO dto)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("FileCrypt", configuration["Email"]));
            emailMessage.To.Add(new MailboxAddress(dto.Username, dto.Email));
            emailMessage.Subject = dto.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = dto.Body
            };

            await smtpClient.EmailSendAsync(emailMessage);
        }
        catch (SmtpClientException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw new SmtpClientException("Error sending message");
        }
    }
}
