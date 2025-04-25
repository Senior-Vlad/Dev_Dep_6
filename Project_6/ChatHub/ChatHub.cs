using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Users.user.Downloads.Dev_Dep_6.Project_6.Controllers;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }
    public async Task SendMessage(string message)
    {
        var HttpContext = Context.GetHttpContext();

        var FirstName = HttpContext?.Session.GetString("FirstName") ?? "user";

        var LastName = HttpContext?.Session.GetString("LastName") ?? "user";
        var role = HttpContext?.Session.GetString("Role") ?? "user";

        var username = $"{FirstName} {LastName} {role}";

        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }

    public async Task JoinGroup(string groupName)
    {
        var HttpContext = Context.GetHttpContext();
        var FirstName = HttpContext?.Session.GetString("FirstName") ?? "user";
        var LastName = HttpContext?.Session.GetString("LastName") ?? "user";
        var username = $"{FirstName} {LastName}";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{username} has joined the group {groupName}.");
    }

    public async Task SendGroupMessage(string groupName, string messageContent)
    {
        var HttpContext = Context.GetHttpContext();

        var userId = HttpContext?.Session.GetInt32("UserId");
        var FirstName = HttpContext?.Session.GetString("FirstName") ?? "user";
        var LastName = HttpContext?.Session.GetString("LastName") ?? "user";
        var Role = HttpContext?.Session.GetString("Role") ?? "user";

        var username = $"{FirstName} {LastName} {Role}";

        var message = new Message
        {
            GroupName = groupName,
            UserId = userId.Value,
            Content = messageContent,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await _messageService.SaveMessageAsync(message);

        await Clients.Group(groupName).SendAsync("ReceiveMessage", username, messageContent, message.Id, message.SentAt.ToString("o"));
    }

    public async Task MarkMessageAsRead(int messageId)
    {
        await _messageService.MarkMessageAsReadAsync(messageId);
        await Clients.All.SendAsync("MessageRead", messageId);
    }
    public async Task GetMessagesForGroup(string groupName)
    {
        var messages = await _messageService.GetMessagesAsync(groupName);

        var formattedMessages = messages.Select(m => new
        {
            User = $"{m.User.UserInfos.FirstOrDefault()?.FirstName ?? "Unknown"} {m.User.UserInfos.FirstOrDefault()?.LastName ?? "Unknown"} {m.User.Role}",
            Content = m.Content,
            SentAt = m.SentAt.ToString("o"),
            MessageId = m.Id,
            IsRead = m.IsRead
        }).ToList();

        await Clients.Caller.SendAsync("ReceiveAllMessages", formattedMessages);
    }


}