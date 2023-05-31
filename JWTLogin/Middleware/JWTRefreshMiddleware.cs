using JWTLogin.JWTService;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace JWTLogin.Middleware;

public class JWTRefreshMiddleware
{
    private readonly RequestDelegate next;
    private readonly IServiceProvider serviceProvider;

    public JWTRefreshMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        this.next = next;
        this.serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        JwtSecurityToken? token = null;
        string? authorization = context.Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        {
            token = new JwtSecurityTokenHandler().ReadJwtToken(authorization.Substring("Bearer ".Length));
        }

        //刷新Token
        if (token != null
            && token.ValidTo > DateTime.UtcNow
            && token.ValidTo.AddMinutes(-30) <= DateTime.UtcNow)
        {
            context.Response.Headers.Add("X-Refresh-Token", await RefreshTokenAsync(token));
        }

        await next.Invoke(context);
    }

    private Task<string> RefreshTokenAsync(JwtSecurityToken token)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var jwtService = serviceScope.ServiceProvider.GetService<IJWTService>();
        var options = serviceScope.ServiceProvider.GetService<IOptionsSnapshot<JWTOptions>>();

        return Task.FromResult(jwtService?.CreateJWT(token.Claims, options?.Value!))!;
    }
}

public static class JWTRefreshMiddlewareExtensions
{
    public static IApplicationBuilder UseJWTRefresh(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JWTRefreshMiddleware>();
    }
}