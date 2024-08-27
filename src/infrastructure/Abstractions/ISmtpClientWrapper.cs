using MimeKit;
using System.Threading;
using System.Threading.Tasks;

namespace infrastructure.Abstractions;

public interface ISmtpClientWrapper
{
    Task EmailSendAsync(MimeMessage message, CancellationToken cancellationToken = default);
}
