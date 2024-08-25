using MimeKit;
using System.Threading.Tasks;

namespace infrastructure.Abstractions;

public interface ISmtpClientWrapper
{
    Task EmailSendAsync(MimeMessage message);
}
