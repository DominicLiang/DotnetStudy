using JWTLogin.JWTService;
using JWTLogin.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace JWTLogin.Filters;

public class JWTRefreshFilter : IAsyncActionFilter
{
    private readonly IJWTService jwtService;
    private readonly IOptionsSnapshot<JWTOptions> options;
    private readonly UserManager<User> manager;

    public JWTRefreshFilter(IJWTService jwtService, IOptionsSnapshot<JWTOptions> options, UserManager<User> manager)
    {
        this.jwtService = jwtService;
        this.options = options;
        this.manager = manager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        JwtSecurityToken? token = null;
        string? authorization = context.HttpContext.Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        {
            token = new JwtSecurityTokenHandler().ReadJwtToken(authorization.Substring("Bearer ".Length));
        }

        //刷新Token
        if (token != null
            && token.ValidTo > DateTime.UtcNow
            && token.ValidTo.AddMinutes(-30) <= DateTime.UtcNow)
        {
            User user = await manager.FindByNameAsync(context.HttpContext.User.Identity?.Name);
            if (user != null)
            {
                string newToken = jwtService.CreateJWT(token.Claims, options.Value);
                context.HttpContext.Response.Headers.Add("X-Refresh-Token", newToken);
                user.Token = newToken;
                await manager.UpdateAsync(user);
            }
        }

        await next();
    }
}
