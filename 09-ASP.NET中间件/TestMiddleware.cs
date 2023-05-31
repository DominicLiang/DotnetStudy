namespace _09_ASP.NET中间件;

// 中间件模板
public class TestMiddleware
{
    private readonly RequestDelegate next;

    public TestMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 前方法
        await context.Response.WriteAsync("TestMiddleware start<br/>");

        await next.Invoke(context);
        
        // 后方法
        await context.Response.WriteAsync("TestMiddleware end<br/>");
    }
}
