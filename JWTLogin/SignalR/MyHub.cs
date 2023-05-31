using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JWTLogin.SignalR;

// signalr验证 也可以放在方法上
[Authorize]
public class MyHub : Hub
{
    public Task SendPublicMsg(string msg)
    {
        string connId = Context.ConnectionId;
        string msgToSend = $"{connId} {DateTime.Now}: {msg}";
        return Clients.All.SendAsync("PublicMsgReceived", msgToSend);
    }
} 