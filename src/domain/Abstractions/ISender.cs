using System.Threading;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface ISender<T> where T : class
{
    Task SendMessage(T message, CancellationToken cancellationToken = default);
}
