using _15_DotnetSpider;
using Serilog;
using Serilog.Events;

ThreadPool.SetMaxThreads(255, 255);
ThreadPool.SetMinThreads(255, 255);

Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Information()
         .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
         .MinimumLevel.Override("System", LogEventLevel.Warning)
         .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
         .Enrich.FromLogContext()
         .WriteTo.Console().WriteTo.RollingFile("logs/spider.log")
         .CreateLogger();

await StudySpider.RunAsync();

Console.WriteLine("Bye!");