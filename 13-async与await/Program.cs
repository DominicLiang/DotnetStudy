namespace _13_async与await;

// 异步方法       C#5.0
// 异步方法一般用async/await来标识
// 约定：方法名一般后带Async
// async/await
// async/await是语法糖，编译器提供的功能
// await 放在task前面（可以用await的方法会表明“可等待”）
// await和async一般成对出现 只有async没有意义 只有await会报错
// await和async要么不用 要用就用到底

public class AsyncAwaitClass
{
    Do d = new Do();

    public void TestShow()
    {
        Test();
        Console.ReadKey();
    }

    private async void Test()
    {
        Console.WriteLine($"当前主线程id：{Thread.CurrentThread.ManagedThreadId}");

        //NoReturnNoAwaitAsync();
        //NoReturnAsync();
        NoReturnTaskAsync();
    }

    private async void NoReturnNoAwaitAsync()
    {
        Console.WriteLine(d.Display("NoReturnNoAwait Start"));

        Task task = Task.Run(() => d.DoSomethingLong());

        Console.WriteLine(d.Display("NoReturnNoAwait End"));
    }

    private async void NoReturnAsync()
    {
        Console.WriteLine(d.Display("NoReturn Start"));

        await Task.Run(() => d.DoSomethingLong());

        // await之后的代码 会自己封装程委托 在task之后回调
        // 这个回调所使用的线程不确定
        Console.WriteLine(d.Display("NoReturn End"));
    }

    private async Task NoReturnTaskAsync()
    {
        Console.WriteLine(d.Display("NoReturnTask Start"));

        Task task = Task.Run(() => d.DoSomethingLong());
        await task;

        Console.WriteLine(d.Display("NoReturnTask End"));
    }

    private async Task<string> WithReturn()
    {
        Console.WriteLine(d.Display("WithReturn Start"));

        string s = await Task.Run(() => d.DoSomethingLong());

        Console.WriteLine(d.Display("WithReturn End"));
        return "###" + s + "###";
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        AsyncAwaitClass a = new AsyncAwaitClass();
        a.TestShow();
    }
}