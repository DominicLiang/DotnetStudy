using JWTLogin.JWTService;
using JWTLogin.Model;
using JWTLogin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

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
}