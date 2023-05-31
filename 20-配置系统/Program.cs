using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace _20_配置系统;

// 官方包
// Microsoft.Extensions.Configuration           配置框架
// Microsoft.Extensions.Configuration.Json      Json支持
// Microsoft.Extensions.Configuration.Binder    Json映射类
// Microsoft.Extensions.Options   
// Microsoft.Extensions.DependencyInjection
class Proxy
{
    public string Address { get; set; }
    public int Port { get; set; }
    public int[] Ids { get; set; }
}

class Config
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Proxy Proxy { get; set; }
}

internal class Program
{
    static void Main(string[] args)
    {
        ConfigurationBuilder builder = new ConfigurationBuilder();
        // 填写配置文件文件名等资料
        builder.AddJsonFile("config.json", optional: true, reloadOnChange: true);
        // 创建根节点
        IConfigurationRoot configRoot = builder.Build();

        //{
        //    // 映射类的做法  简单快捷
        //    Config config = configRoot.Get<Config>()!;
        //    Console.WriteLine(config.Name);
        //    Console.WriteLine(config.Age);
        //    Console.WriteLine(config.Proxy.Address);
        //    Console.WriteLine(config.Proxy.Port);
        //}
        {
            // DI
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<TestController>();

            // !!!! 
            services.Configure<Config>(e => configRoot.Bind(e));

            using (ServiceProvider sp = services.BuildServiceProvider())
            {
                using (var scope = sp.CreateScope())
                {
                    var testController = scope.ServiceProvider.GetRequiredService<TestController>();
                    testController.Test();
                }
            }
        }
    }
}

class TestController
{
    private readonly IOptionsSnapshot<Config> optConfig;

    public TestController(IOptionsSnapshot<Config> optConfig)
    {
        this.optConfig = optConfig;
    }

    public void Test()
    {
        Config config = optConfig.Value;
        Console.WriteLine(config.Age);
        Console.WriteLine(string.Join(",", config.Proxy.Ids));
    }
}

