using api.Utils;
using application.Workflows.Chat;
using common.DTO.ModelDTO.Chat;
using datahub.Redis;
using domain.Abstractions;
using domain.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace api.Hubs;

[Authorize]
public class ChatHub(
    ChatWk chatWorkflow,
    MessageWk messageWorkflow,
    IDataCache<RedisChatConnection> users,
    IUserInfo userInfo) : Hub
{
    public override async Task OnConnectedAsync()
    {
        await users.SetAsync(userInfo.UserId.ToString(), Context.ConnectionId, TimeSpan.FromDays(30));
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await users.DeleteSingleAsync(userInfo.UserId.ToString());
        await base.OnDisconnectedAsync(exception);
    }

    [Authorize(Roles = "Employer")]
    public async Task CreateChat(int applicantId, string name)
    {
        await chatWorkflow.AddSingle(new ChatDTO
        {
            CandidateId = applicantId,
            EmployerId = userInfo.UserId,
            EmployerName = name
        });
    }

    public async Task SendMessage(Message message)
    {
        message.MessageDTO.EmployerId = message.AsEmployer ? userInfo.UserId : message.MessageDTO.EmployerId;
        message.MessageDTO.CandidateId = !message.AsEmployer ? userInfo.UserId : message.MessageDTO.CandidateId;
        var response = await messageWorkflow.AddSingle(message.MessageDTO);
        if (response.IsSuccess && response.ObjectData is ChatWithMessage chat)
        {
            var name = message.AsEmployer ? chat.Chat.EmployerName : chat.Chat.CandidateName;
            await Clients.Group(chat.Chat.ChatId.ToString()).SendAsync("ReceiveMessage", name, message.MessageDTO.Message);
        }
    }

    public async Task JoinChat(int chatId)
    {
        var response = await chatWorkflow.GetSingle(chatId, userInfo.UserId);
        if (response.IsSuccess && response.ObjectData is ChatModel chat)
        {
            var employer = await users.GetSingleAsync<int>(chat.EmployerId.ToString());
            if (employer != default)
                await Groups.AddToGroupAsync(employer.ToString(), chat.ChatId.ToString());

            var candidate = await users.GetSingleAsync<int>(chat.CandidateId.ToString());
            if (candidate != default)
                await Groups.AddToGroupAsync(candidate.ToString(), chat.ChatId.ToString());
        }
    }

    public async Task LeaveChat(int chatId)
    {
        var response = await chatWorkflow.RemoveSingle(chatId, userInfo.UserId);
        if (response.IsSuccess && response.ObjectData is ChatModel chat)
        {
            var employer = await users.GetSingleAsync<int>(chat.EmployerId.ToString());
            if (employer != default)
                await Groups.RemoveFromGroupAsync(employer.ToString(), chat.ChatId.ToString());

            var candidate = await users.GetSingleAsync<int>(chat.CandidateId.ToString());
            if (candidate != default)
                await Groups.RemoveFromGroupAsync(candidate.ToString(), chat.ChatId.ToString());
        }
    }

    private class ChatWithMessage
    {
        public MessageModel Message { get; set; } = null!;
        public ChatModel Chat { get; set; } = null!;
    }
}

public class Message
{
    [Required]
    public MessageDTO MessageDTO { get; set; } = null!;

    [Required]
    public bool AsEmployer { get; set; }
}
