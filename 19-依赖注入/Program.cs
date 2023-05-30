using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

namespace _19_依赖注入;

// 官方DI包
// Microsoft.Extensions.DependencyInjection

public interface ITestScopedService
{
    public string Name { get; set; }
    public void SayHi();
}

public interface ITestSingletonService
{
    public string Name { get; set; }
    public void SayHi();
}

public interface ITestTransientService
{
    public string Name { get; set; }
    public void SayHi();
}


public class TestScopedService : ITestScopedService
{
    public string Name { get; set; }
    public string Test { get; set; }

    public void SayHi()
    {
        Console.WriteLine($"你好，我是{Name}");
    }
}

public class TestSingletonService : ITestSingletonService
{
    public string Name { get; set; }
    public string Test { get; set; }

    public void SayHi()
    {
        Console.WriteLine($"你好，我是{Name}");
    }
}

public class TestTransientService : ITestTransientService
{
    public string Name { get; set; }
    public string Test { get; set; }

    public void SayHi()
    {
        Console.WriteLine($"你好，我是{Name}");
    }
}

public interface ITestServiceZero
{
    void Display();
}

public interface ITestServiceOne
{
    void Display();
}

public interface ITestServiceTwo
{
    void Display();
}

public interface ITestServiceThree
{
    void Display();
}

public class TestServiceZero : ITestServiceZero
{
    private readonly ITestServiceOne testServiceOne;

    public TestServiceZero(ITestServiceOne testServiceOne)
    {
        this.testServiceOne = testServiceOne;
    }

    public void Display()
    {
        Console.WriteLine("TestServiceZero");
        testServiceOne.Display();
    }
}

public class TestServiceOne : ITestServiceOne
{
    private readonly ITestServiceTwo testServiceTwo;
    private readonly ITestServiceThree testServiceThree;

    public TestServiceOne(ITestServiceTwo testServiceTwo, ITestServiceThree testServiceThree)
    {
        this.testServiceTwo = testServiceTwo;
        this.testServiceThree = testServiceThree;
    }

    public void Display()
    {
        Console.WriteLine("TestServiceOne");
        testServiceTwo.Display();
        testServiceThree.Display();
    }
}

public class TestServiceTwo : ITestServiceTwo
{
    public void Display()
    {
        Console.WriteLine("TestServiceTwo");
    }
}

public class TestServiceThree : ITestServiceThree
{
    public void Display()
    {
        Console.WriteLine("TestServiceThree");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        {
            //ServiceCollection services = new ServiceCollection();

            //services.AddScoped<ITestScopedService, TestScopedService>();
            //services.AddSingleton<ITestSingletonService, TestSingletonService>();
            //services.AddTransient<ITestTransientService, TestTransientService>();

            //using (ServiceProvider sp = services.BuildServiceProvider())
            //{
            //    // Scoped    指定范围内唯一(ASP.NET的话是一次请求为一个Scoped周期)
            //    ITestScopedService ScopedT1;
            //    using (IServiceScope s = sp.CreateScope())
            //    {
            //        ITestScopedService Scoped = s.ServiceProvider.GetService<ITestScopedService>()!;
            //        Scoped.Name = "Scoped";
            //        Scoped.SayHi();

            //        ITestScopedService Scoped2 = s.ServiceProvider.GetService<ITestScopedService>()!;
            //        Scoped2.Name = "Scoped";
            //        Scoped2.SayHi();
            //        Console.WriteLine(ReferenceEquals(Scoped, Scoped2));
            //        ScopedT1 = Scoped;
            //    }

            //    ITestScopedService ScopedT2;
            //    using (IServiceScope s = sp.CreateScope())
            //    {
            //        ITestScopedService Scoped3 = s.ServiceProvider.GetService<ITestScopedService>()!;
            //        Scoped3.Name = "Scoped";
            //        Scoped3.SayHi();

            //        ScopedT2 = Scoped3;
            //    }

            //    Console.WriteLine(ReferenceEquals(ScopedT1, ScopedT2));


            //    // Singleton 全局唯一
            //    ITestSingletonService Singleton = sp.GetService<ITestSingletonService>()!;
            //    Singleton.Name = "Singleton";
            //    Singleton.SayHi();

            //    ITestSingletonService Singleton2 = sp.GetService<ITestSingletonService>()!;
            //    Singleton2.Name = "Singleton2";
            //    Singleton2.SayHi();

            //    Singleton.SayHi();

            //    Console.WriteLine(ReferenceEquals(Singleton, Singleton2));

            //    // Transient  每次不一样
            //    ITestTransientService Transient = sp.GetService<ITestTransientService>()!;
            //    Transient.Name = "Transient";
            //    Transient.SayHi();

            //    ITestTransientService Transient2 = sp.GetService<ITestTransientService>()!;
            //    Transient2.Name = "Transient2";
            //    Transient2.SayHi();

            //    Transient.SayHi();

            //    Console.WriteLine(ReferenceEquals(Transient, Transient2));
            //}
        }
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ITestServiceZero, TestServiceZero>();
            services.AddSingleton<ITestServiceOne, TestServiceOne>();
            services.AddSingleton<ITestServiceTwo, TestServiceTwo>();
            services.AddSingleton<ITestServiceThree, TestServiceThree>();

            using (var service = services.BuildServiceProvider())
            {
                // service.GetRequiredService创建起点
                ITestServiceZero zero = service.GetRequiredService<ITestServiceZero>();
                zero.Display();
            }
        }
    }
}