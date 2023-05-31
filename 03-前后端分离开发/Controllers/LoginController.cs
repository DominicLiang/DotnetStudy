using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _03_前后端分离开发.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    public LoginReponse Login(LoginRequest request)
    {
        if (request.UserName == "admin" && request.Password == "123456")
        {
            var items = Process.GetProcesses().Select(p => new ProcessInofo(p.Id, p.ProcessName, p.WorkingSet64));
            return new LoginReponse(true, items.ToArray());
        }
        else
        {
            return new LoginReponse(false, null);
        }
    }
}

public record LoginRequest(string UserName, string Password);
public record ProcessInofo(long Id, string Name, long WorkingSet);
public record LoginReponse(bool Ok, ProcessInofo[]? ProcessInofos);
