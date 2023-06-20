using JWTLogin.JWTService;
using JWTLogin.Model;
using JWTLogin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Timer = System.Timers.Timer;

namespace JWTLogin.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class Test : ControllerBase
{
    private readonly ILogger<Test> _logger;
    private readonly IJWTService _jwtService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IOptionsSnapshot<JWTOptions> _jwtOptions;

    public Test(ILogger<Test> logger, IJWTService jwtService, UserManager<User> userManager, RoleManager<Role> roleManager, IOptionsSnapshot<JWTOptions> jwtOptions)
    {
        _logger = logger;
        _jwtService = jwtService;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtOptions = jwtOptions;
    }

    private async Task CreateBaseRole()
    {
        if (!await _roleManager.RoleExistsAsync("guess"))
        {
            var guess = new Role() { Name = "guess" };
            await _roleManager.CreateAsync(guess);
        }
        if (!await _roleManager.RoleExistsAsync("member"))
        {
            var member = new Role() { Name = "member" };
            await _roleManager.CreateAsync(member);
        }
        if (!await _roleManager.RoleExistsAsync("admin"))
        {
            var admin = new Role() { Name = "admin" };
            await _roleManager.CreateAsync(admin);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Hello()
    {
        await CreateBaseRole();
        return Ok("hello");
    }

    [HttpPost]
    public async Task<IActionResult> Registry(UserInfoViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userManager.CreateAsync(new User()
            {
                UserName = model.UserName,
            }, model.Password);

            if (result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(model.UserName);
                await _userManager.AddToRoleAsync(user, "member");
            }

            if (result.Succeeded)
            {
                return Ok(result);
            }
        }

        return BadRequest(ModelState);
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string token = _jwtService.CreateJWT(claims, _jwtOptions.Value);
            user.Token = token;
            await _userManager.UpdateAsync(user);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Refresh-Token");
            HttpContext.Response.Headers.Add("X-Refresh-Token", token);
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Console.WriteLine("***********In***********");
        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
        if (user != null)
        {
            user.Token = string.Empty;
            await _userManager.UpdateAsync(user);
            Console.WriteLine("***********OK***********");
            return Ok();
        }
        return Unauthorized();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> MakeAdmin()
    {
        if (User.IsInRole("admin")) return Ok("你已经是管理员了");
        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, "admin");
            if (result.Succeeded) return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveAdmin()
    {
        if (!User.IsInRole("admin")) return Ok("你不是管理员");
        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
        if (user != null)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, "admin");
            if (result.Succeeded) return Ok();
        }
        return BadRequest();
    }

    [HttpGet]
    public IActionResult ContentNoNeedLogin()
    {
        return Ok("ContentNoNeedLogin  正常连接了！");
    }

    [HttpGet]
    [Authorize(Roles = "member")]
    public IActionResult ContentNeedLogin()
    {
        return Ok("ContentNeedLogin  正常连接了！");
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult ContentNeedAdmin()
    {
        return Ok("ContentNeedAdmin  正常连接了！");
    }

    //private User user = new User()
    //{
    //    Id=Guid.NewGuid(),
    //    UserName = "admin",
    //    PhoneNumber = "1234567890",
    //    Email="test@test.com",
    //    SecurityStamp="sdffffffffffff"
    //};

    [HttpGet]
    public async Task<IActionResult> GenerateUserTokenAsync()
    {
        User user = await _userManager.FindByNameAsync("hello");
        if (user == null) return Ok("no user");
        var res = await _userManager.GenerateChangePhoneNumberTokenAsync(user, "1234567");
        //var res = await _userManager.GenerateUserTokenAsync(user, "MyTokenProvider", "PhoneNumberConfirmation");
        //var encodedToken = HttpUtility.UrlEncode(res);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> VerifyUserTokenAsync(string token)
    {
        User user = await _userManager.FindByNameAsync("hello");
        if (user == null) return Ok("no user");
        var res = await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, "1234567");
        return Ok(res);
    }
}

public class MyTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
{
    private static Dictionary<string, TokenData> Tokens = new();

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(false);
    }

    public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        string key = await GetKey(purpose, manager, user);
        string token = GenerateRandomCode(6).ToString();

        if (Tokens.ContainsKey(key))
        {
            TokenData data = Tokens[key];
            data.Token = token;
            data.Timer.Stop();
            data.Timer.Start();
        }
        else
        {
            Timer timer = new Timer(10000);
            timer.Elapsed += (s, e) =>
            {
                Tokens.Remove(key);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
            Tokens.Add(key, new TokenData(token, timer));
        }

        return await Task.FromResult(token);
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        string key = await GetKey(purpose, manager, user);
        if (!Tokens.TryGetValue(key, out TokenData? data)) return false;
        bool result = data.Token == token;
        if (result) Tokens.Remove(key);
        return await Task.FromResult(result);
    }

    private async Task<string> GetKey(string purpose, UserManager<TUser> manager, TUser user)
    {
        var userId = await manager.GetUserIdAsync(user);
        string? stamp = null;
        if (manager.SupportsUserSecurityStamp)
        {
            stamp = await manager.GetSecurityStampAsync(user);
        }
        return $"{userId}-{stamp}-{purpose}";
    }

    private string GenerateRandomCode(int length)
    {
        var result = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            result.Append(Random.Shared.Next(0, 10).ToString());
        }
        return result.ToString();
    }

    class TokenData
    {
        public string Token { get; set; }
        public Timer Timer { get; set; }

        public TokenData(string token, Timer timer)
        {
            Token = token;
            Timer = timer;
        }
    }
}

public class InvitationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
    public InvitationTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<InvitationTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
    {

    }
}

public class InvitationTokenProviderOptions : DataProtectionTokenProviderOptions
{
}