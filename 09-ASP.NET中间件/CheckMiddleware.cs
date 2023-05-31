using Dynamic.Json;
using System.Text.Json;

namespace _09_ASP.NET中间件;

public class CheckMiddleware
{
    private readonly RequestDelegate next;

    public CheckMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string password = context.Request.Query["password"];
        if (password == "123")
        {
            if (context.Request.HasJsonContentType())
            {
                Stream stream = context.Request.BodyReader.AsStream();
                dynamic objJson = await DJson.ParseAsync(stream);
                context.Items["BodyJson"] = objJson;
            }
            await next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = 401;
        }
    }
}