using System.Security.Claims;
using JWTLogin.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace JWTLogin.SignalR;

// signalr验证 也可以放在方法上
[Authorize]
public class MyHub : Hub
{
    private readonly UserManager<User> userManager;

    public MyHub(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    

    public async Task SendPrivateMsg(string toUserName, string message)
    {
        var user = await userManager.FindByNameAsync(toUserName);
        string id = user.Id.ToString().ToUpper();
        string? currentUserName = Context.User.FindFirst(ClaimTypes.Name).Value;

        await Clients.Client(Context.ConnectionId).SendAsync("PrivateMsgRecevied", currentUserName, message);
        Console.WriteLine($"id:{id}  name:{currentUserName}");
    }

    public Task SendPublicMsg(string msg)
    {
        string connId = Context.ConnectionId;
        string msgToSend = $"{connId} {DateTime.Now}: {msg}";

        return Clients.All.SendAsync("PublicMsgReceived", msgToSend);
    }
}