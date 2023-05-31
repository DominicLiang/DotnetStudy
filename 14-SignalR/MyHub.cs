using Microsoft.AspNetCore.SignalR;

namespace _14_SignalR;

public class MyHub : Hub
{
    public Task SendPublicMsg(string msg)
    {
        string connId = Context.ConnectionId;
        string msgToSend = $"{connId} {DateTime.Now}: {msg}";
        return Clients.All.SendAsync("PublicMsgReceived", msgToSend);
    }
}
