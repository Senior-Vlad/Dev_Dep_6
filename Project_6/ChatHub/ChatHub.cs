using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
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

    public async Task SendGroupMessage(string groupName, string message)
    {
        var HttpContext = Context.GetHttpContext();


        var FirstName = HttpContext?.Session.GetString("FirstName") ?? "user";
        var LastName = HttpContext?.Session.GetString("LastName") ?? "user";
        var Role = HttpContext?.Session.GetString("Role") ?? "user";

        var username = $"{FirstName} {LastName} {Role}";

        await Clients.Group(groupName).SendAsync("ReceiveMessage", username, message);
    }

}