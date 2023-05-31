using _09_ASP.NET中间件;

// 中间件的三个概念
// Map、Use和Run
// Map用来定义一个管道可以处理哪些请求
// Use和Run用来定义管道
// 一个管道由若干个Use和一个Run组成
// 每个Use引入一个中间件
// 而Run是用来执行最终的核心应用逻辑

// Filter与Middleware的区别
// 中间件是ASP.NET Core这个基础提供的功能，而Filter是ASP.NET Core MVC中提供的功能
// ASP.NET Core MVC是由MVC中间件提供的框架，而Filter属于MVC中间件提供的功能

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 引入中间件
app.Map("/test", async (pipeBuilder) =>
{
    pipeBuilder.UseMiddleware<CheckMiddleware>();
    pipeBuilder.Run(async context =>
    {
        dynamic? obj = context.Items["BodyJson"];
        if (obj != null)
        {
            await context.Response.WriteAsync($"{obj}+<br/>");
        }
    });
});

app.Run();
