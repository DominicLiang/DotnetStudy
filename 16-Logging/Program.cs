using Exceptionless;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using SystemServices;

namespace _16_Logging;

// *官方包

// Microsoft.Extensions.Logging            基础包
// Microsoft.Extensions.Logging.Console    输出到控制台
// Microsoft.Extensions.Logging.EventLog   输出到事件查看器 Windows专用
// .net没有官方的文本日志提供者 NLog
// 日志级别
// Trace < Debug < Information < Warning < Error < Critical

// 文本日志  小体量可以

// *第三方包 NLog
// NLog.Extensions.Logging                 文本日志提供者
// 配置看nlog.config
// 配置文档 👇
// https://github.com/NLog/NLog.Extensions.Logging

// 本文日志一般按日期区分
// 限制日志总个数
// 限制单个文件大小
// 日志分类
// 不同级别或者不同模块的日志记录到不同的地方
// 日志过滤
// 项目不同阶段需要记录的日志不同，严重错误可以调用短信Provider

// *集中化日志
// 第三方包 Serilog
// Serilog.AspNetCore
// Serilog.Sinks.Exceptionless
// https://exceptionless.com/


class Test1
{
    private readonly ILogger<Test1> logger;
    public Test1(ILogger<Test1> logger)
    {
        this.logger = logger;
    }

    public void Test()
    {
        logger.LogDebug("开始执行数据库同步");
        logger.LogDebug("连接数据库成功");
        logger.LogWarning("查找数据失败，重试第一次");
        logger.LogWarning("查找数据失败，重试第二次");
        logger.LogError("查找数据最终失败");
        try
        {
            File.ReadAllText("");
            logger.LogDebug("读取文件成功");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "读取文件失败");
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        ExceptionlessClient.Default.Startup("IcYIpgE7MsqudVDTVcZdDBdyZBhtNdLoQaxWGyzn");

        ServiceCollection services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            //builder.AddConsole();//输出到控制台
            //builder.AddEventLog();//注意：Windows专用
            //builder.SetMinimumLevel(LogLevel.Trace);//设置最低输出级别

            ////NLog文本日志
            //builder.AddNLog();

            //Serilog
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console(new JsonFormatter())
            .WriteTo.File(new JsonFormatter(), "logs/Serilog.log")
            .WriteTo.Exceptionless()
            .CreateLogger();

            builder.AddSerilog();
        });
        services.AddSingleton<Test1>();
        services.AddSingleton<Test2>();

        using (var sp = services.BuildServiceProvider())
        {
            var test1 = sp.GetRequiredService<Test1>();
            var test2 = sp.GetRequiredService<Test2>();

            test1.Test();
            test2.Test();
        }
    }
}