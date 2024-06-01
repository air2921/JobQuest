using System.Threading.Tasks;

namespace application.Abstractions.Infrastructure
{
    public interface ISender<T>
    {
        Task SendMessage(T message);
    }
}
