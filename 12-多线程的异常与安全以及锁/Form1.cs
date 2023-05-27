namespace _12_多线程的异常与安全以及锁;

// 1.异常处理、线程取消、多线程的临时变量
// 2.线程的安全和锁lock

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // 异常处理
    // 多线程的异常不能跨线程catch，线程内的异常线程内解决
    private void button1_Click(object sender, EventArgs e)
    {
        try
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 20; i++)
            {
                string name = $"ThreadException {i}";
                Action<object> act = t =>
                {
                    try
                    {
                        Thread.Sleep(1000);
                        if (t.ToString()!.Equals("ThreadException 11"))
                        {
                            throw new Exception($"{t} 执行失败");
                        }
                        if (t.ToString()!.Equals("ThreadException 12"))
                        {
                            throw new Exception($"{t} 执行失败");
                        }
                        Console.WriteLine($"{t} 执行成功");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
                tasks.Add(Task.Run(() => act(name)));
            }
            Task.WaitAll(tasks.ToArray());
        }
        catch (AggregateException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    // 线程取消
    // 多个线程并发，某个失败后，通知所有线程都停下来
    // Task无法从外部终止，Thread.Abort不靠谱
    // 可以线程自己停止自己 => 公共访问变量 => 修改公共变量 => 线程不断检测变量
    // CancellationTokenSource去标识任务是否取消
    // Token 启动Task的时候传入,如果cancel,这个任务会放弃启动并抛出异常
    private void button2_Click(object sender, EventArgs e)
    {
        List<Task> tasks = new List<Task>();

        // 在线程外部声明公开变量CancellationTokenSource
        CancellationTokenSource cts = new CancellationTokenSource();

        try
        {
            for (int i = 0; i < 40; i++)
            {
                string name = $"ThreadException {i}";
                Action<object?> act = t =>
                {
                    try
                    {
                        Thread.Sleep(2000);
                        if (t.ToString()!.Equals("ThreadException 11"))
                        {
                            throw new Exception($"{t} 执行失败");
                        }
                        if (t.ToString()!.Equals("ThreadException 12"))
                        {
                            throw new Exception($"{t} 执行失败");
                        }

                        // 如果公共变量标识取消就用return取消线程
                        if (cts.IsCancellationRequested)
                        {
                            Console.WriteLine($"{t} 放弃执行任务");
                            return;
                        }
                        else
                        {
                            Console.WriteLine($"{t} 执行成功");
                        }
                    }// 如果抓到异常,就用CancellationTokenSource的Cancel方法标识取消
                    catch (Exception ex)
                    {
                        cts.Cancel();
                        Console.WriteLine(ex.Message);
                    }
                };
                // Task.Factory的StartNew方法可以接收CancellationToken
                tasks.Add(Task.Factory.StartNew(act, name, cts.Token));
            }
            Task.WaitAll(tasks.ToArray());
        }
        catch (AggregateException aex)
        {
            foreach (Exception item in aex.InnerExceptions)
            {
                Console.WriteLine(item.Message);
            }
        }
    }

    // 多线程临时变量
    private void button3_Click(object sender, EventArgs e)
    {
        //i最后是5 全程只有唯一一个i  在Sleep之后i早就到5了
        //k       全程5个k         所以数值可以独立
        for (int i = 0; i < 5; i++)
        {
            int k = i;
            Task.Run(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"k = {k}     i = {i}");
            });
        }
    }

    // 线程安全
    // 共有变量:都能访问局部变量/全局变量/数据库的值/硬盘文件
    // 线程内部不共享的是安全
    private static readonly object coreLock = new object();
    // 安全队列 ConcurrentQueue 一个线程去完成操作

    // 不要冲突 => 数据拆分,避免冲突
    // 拆分数据给不同的线程,所有线程完成时whenall合拼数据
    private void button4_Click(object sender, EventArgs e)
    {
        int totalCount = 0;
        List<Task> tasks = new List<Task>();
        List<int> ints = new List<int>();
        for (int i = 0; i < 10000; i++)
        {
            int newI = i;
            tasks.Add(Task.Factory.StartNew(() =>
            {
                // 每次实例化是不同的锁,同一个实例是相同的锁
                // 但是这个实例别人也能访问,别人也能锁
                lock (this)
                {
                }

                lock (coreLock)
                {
                    totalCount++;
                    ints.Add(newI);
                }
            }));
        }
        Task.WaitAll(tasks.ToArray());
        Console.WriteLine(totalCount);
        Console.WriteLine(ints.Count());
    }
}