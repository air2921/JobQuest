using System.Threading.Tasks;

namespace domain.Abstractions;

public interface ISender<T>
{
    Task SendMessage(T message);
}
