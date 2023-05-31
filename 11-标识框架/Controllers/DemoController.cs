using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Net.Http;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace _11_标识框架.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DemoController : ControllerBase
{
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly UserManager<MyUser> userManager;
    private readonly RoleManager<MyRole> roleManager;

    public DemoController(UserManager<MyUser> userManager, RoleManager<MyRole> roleManager, IWebHostEnvironment hostEnvironment)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.hostEnvironment = hostEnvironment;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Test1()
    {
        if (await roleManager.RoleExistsAsync("admin") == false)
        {
            MyRole role = new MyRole() { Name = "admin" };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded) return BadRequest("Create Role Failed");
        }
        MyUser user1 = await userManager.FindByNameAsync("yzk");
        Console.WriteLine(user1);
        if (user1 == null)
        {
            user1 = new MyUser() { UserName = "yzk", Email = "dominic1987@foxmail.com" };

            var result = userManager.CreateAsync(user1, "JASIjfidc123");
            if (!result.IsCompletedSuccessfully) return BadRequest("Create User Failed");
        }
        if (!await userManager.IsInRoleAsync(user1, "admin"))
        {
            await userManager.AddToRoleAsync(user1, "admin");
        }

        return "OK";
    }

    [HttpPost]
    public async Task<ActionResult> CheckPwd(CheckPwdRequest req)
    {
        string userName = req.UserName;
        string pwd = req.Password;
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            if (hostEnvironment.IsDevelopment())
            {
                return BadRequest("用户名不存在");
            }
            else
            {
                return BadRequest("用户名或密码错误");
            }
        }
        else
        {
            if (await userManager.IsLockedOutAsync(user))
            {
                return BadRequest("用户被锁定 锁定结束时间" + user.LockoutEnd);
            }
            if (await userManager.CheckPasswordAsync(user, pwd))
            {
                await userManager.ResetAccessFailedCountAsync(user);
                return Ok("登录成功");
            }
            else
            {
                await userManager.AccessFailedAsync(user);
                return BadRequest("用户名或密码错误");
            }
        }
    }

    [HttpPost]
    public async Task<ActionResult> SendResetPasswordToken(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("用户名不存在");
        }
        else
        {
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            // TODO send email
            return Ok();
        }
    }

    private async Task SendEmail()
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("[REPLACE WITH YOUR EMAIL]", "[REPLACE WITH YOUR NAME]"),
            Subject = "Sending with Twilio SendGrid is Fun",
            PlainTextContent = "and easy to do anywhere, especially with C#"
        };
        msg.AddTo(new EmailAddress("[REPLACE WITH DESIRED TO EMAIL]", "[REPLACE WITH DESIRED TO NAME]"));
        var response = await client.SendEmailAsync(msg);
    }

    [HttpPut]
    public async Task<ActionResult> ResetPassword(string email, string token, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("用户不存在");
        }
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        if (result.Succeeded)
        {
            await userManager.ResetAccessFailedCountAsync(user);
            return Ok();
        }
        else
        {
            await userManager.AccessFailedAsync(user);
            return BadRequest("密码重置失败");
        }
    }
}
