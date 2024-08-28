using api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace api.Hubs;

[Authorize]
public class ChatHub(IUserInfo user) : Hub<IChatRoom>
{

}

public interface IChatRoom
{
    Task ReceiveMessage(int userId, int chat, string message);
}
