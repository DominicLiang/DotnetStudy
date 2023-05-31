using JWTLogin.Migrations;
using JWTLogin.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace JWTLogin.Filters;

public class JWTValidationFilter : IAsyncActionFilter
{
    private readonly UserManager<User> manager;

    public JWTValidationFilter(UserManager<User> manager)
    {
        this.manager = manager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string tokenInContext = string.Empty;
        string? authorization = context.HttpContext.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        {
            tokenInContext = authorization.Substring("Bearer ".Length);
        }
        if (context.HttpContext.User.Identity?.Name != null)
        {
            User user = await manager.FindByNameAsync(context.HttpContext.User.Identity?.Name);

            if (user.Token == null || user.Token == string.Empty || user.Token != tokenInContext)
            {
                var result = new ObjectResult("some msg");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = result;
                return;
            }
        }
        await next();
        return;
    }
}
