using System.Diagnostics;

namespace _11__异步和多线程的发展
{
    public partial class Form1 : Form
    {
        Class1 c = new Class1();
        int number = 0;

        public Form1()
        {
            InitializeComponent();
        }

        // Thread       .net1.0
        // *默认前台线程 不推荐使用
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(c.Display("ClickIn"));

            Thread thread = new Thread(() =>
            {
                string s = c.DoSomethingLong();
                Console.WriteLine(s);
            });

            thread.IsBackground = true;
            //默认是前台线程
            //前台线程:
            //线程必须完成任务才退出,尽管进程关闭还继续运行
            //后台线程:
            //随进程关闭而关闭

            //thread.Priority = ThreadPriority.Highest;
            //线程优先级 作用不大

            thread.Start();
            Console.WriteLine(c.Display("ClickOut"));

            //Thread.Sleep(1000);
            //当前线程挂起1秒,1秒内什么都不干

            //thread.Join();
            //可以理解为await
            //后面的代码会等到thread创建的线程完成后再运行
            //主线程等待新线程完成 期间不能做其他工作(卡界面)
            //thread.Join(2000);
            //这个主线程等待新线程两秒完成 等待两秒期间主线程不能做其他工作

            //Thread.Yield();
            //yield: 释放我获得到的CPU时间片；但是释放了CPU时间片，状态依然是Runnable，因为他释放时间片，并不会释放锁，也不会陷入阻塞，下一次CPU调度，依然可能随时把他给调度起来。JVM不保证yield原则，比如没有其他使用我的CPU资源，我调用yield，JVM也许并不会将我的CPU 释放掉，一般我们开发不使用这个方法。
        }

        // ThreadPool   .net2.0
        // 1.Thread提供了太多Api(很多弃用)
        // 2.避免无限使用线程,加以限制
        // 3.重用线程,避免重复创建和销毁
        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(c.Display("ClickIn"));

            ThreadPool.QueueUserWorkItem(t =>
            {
                string s = c.DoSomethingLong();
                Console.WriteLine(s);
            });

            ThreadPool.GetMaxThreads(out int a1, out int b1);
            Console.WriteLine($"线程池中辅组线程的最大数目:{a1}   线程池中异步I/O线程的最大数目:{b1}");

            ThreadPool.GetMinThreads(out int a2, out int b2);
            Console.WriteLine($"线程池中辅组线程的最小数目:{a2}   线程池中异步I/O线程的最小数目:{b2}");

            // 设置线程池的最大和最小线程数
            ThreadPool.SetMaxThreads(16, 16);
            ThreadPool.SetMinThreads(16, 16);

            Console.WriteLine(c.Display("ClickOut"));
        }

        // Task         .net3.0
        // *可await
        // *基于线程池 多API
        // *主流多线程方法 推荐使用
        private void button5_Click(object sender, EventArgs e)
        {
            Console.WriteLine(c.Display("ClickIn"));

            //Task.Run(() =>
            //{
            //    string s = c.DoSomethingLong();
            //    Console.WriteLine(s);
            //});

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    string s = c.DoSomethingLong(100, 1000);
                    Console.WriteLine(s);
                }));
            }

            //Task.Run(() =>
            //{
            //    //WaitAll
            //    //阻塞线程 直到task列表里的所有任务都完成
            //    Task.WaitAll(tasks.ToArray());
            //    Console.WriteLine(c.Display("*** All Finish! ***"));
            //});
            //Task.Run(() =>
            //{
            //    //WaitAny
            //    //阻塞线程 直到task列表里的任一任务完成
            //    Task.WaitAny(tasks.ToArray());
            //    Console.WriteLine(c.Display("*** One Finish! ***"));
            //}); 

            //WhenAll / WhenAny
            //不阻塞线程,在任务完成后用ContinueWith执行回调
            Task.WhenAll(tasks).ContinueWith(t =>
            {
                Console.WriteLine(c.Display("*** All Finish! ***"));
            });

            //SleepAndDelay();
            //Task.Run(() => TaskControlMaxThread(10));

            Console.WriteLine(c.Display("ClickOut"));
        }

        public void SleepAndDelay()
        {
            //Stopwatch计算耗时的类
            Stopwatch sw = Stopwatch.StartNew();
            //Thread.Sleep(5000);   //Sleep直接挂起线程
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);

            //Task.Delay不会挂起线程,只是延时回调
            Task.Delay(5000).ContinueWith(t =>
            {
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            });
        }

        public void TaskControlMaxThread(int maxThread)
        {
            List<int> nums = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                nums.Add(i);
            }
            Action<int> action = i =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(new Random(i).Next(100, 300));
            };
            List<Task> tasks = new List<Task>();
            foreach (int n in nums)
            {
                int k = n;
                tasks.Add(Task.Run(() => action.Invoke(k)));
                if (tasks.Count > maxThread)
                {
                    Task.WaitAny(tasks.ToArray());
                    tasks = (from t in tasks
                             where t.Status != TaskStatus.RanToCompletion
                             select t).ToList();
                }
            }
            Task.WhenAll(tasks.ToArray()).ContinueWith(task => c.Display("All Finish"));
        }

        // TaskFactory  .net4.0
        // *可await
        private void button6_Click(object sender, EventArgs e)
        {
            TaskFactory taskFactory = Task.Factory;
            List<Task> tasks = new List<Task>()
            {
                taskFactory.StartNew(o =>
                {
                    string s = c.DoSomethingLong(100,1000);
                    Console.WriteLine(s);
                }, "Task1"),
                taskFactory.StartNew(o =>
                {
                    string s = c.DoSomethingLong(100,1000);
                    Console.WriteLine(s);
                }, "task2"),
                taskFactory.StartNew(o =>
                {
                    string s = c.DoSomethingLong(100,1000);
                    Console.WriteLine(s);
                }, "task3")
            };
            taskFactory.ContinueWhenAny(tasks.ToArray(), t =>
            {
                Console.WriteLine(t.AsyncState);
            });
            taskFactory.ContinueWhenAll(tasks.ToArray(), t =>
            {
                Console.WriteLine(t[0].AsyncState);
            });
        }

        // Parallel     .net4.0
        private void button7_Click(object sender, EventArgs e)
        {
            Console.WriteLine(c.Display("ClickIn"));
            {
                Task.Run(() =>
                {
                    //Parallel直接在主线程运行的话主线程会挂起
                    //Parallel.Invoke(new Action[]
                    //{
                    //    () =>
                    //    {
                    //        string s = c.DoSomethingLong(100, 1000);
                    //        Console.WriteLine(s);
                    //    },
                    //    () =>
                    //    {
                    //        string s = c.DoSomethingLong(100, 1000);
                    //        Console.WriteLine(s);
                    //    },
                    //    () =>
                    //    {
                    //        string s = c.DoSomethingLong(100, 1000);
                    //        Console.WriteLine(s);
                    //    },
                    //});

                    //int[] ints = { 1, 2, 3, 4, 5 };
                    //Parallel.ForEach(ints, i =>
                    //{
                    //    string s = c.DoSomethingLong(100, 1000);
                    //    Console.WriteLine(s);
                    //});

                    //Parallel控制最大线程数非常简单 只要加一个Option
                    ParallelOptions options = new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = 2
                    };
                    Parallel.For(0, 10, options, (i,state) =>
                    {
                        //注意 break和stop不能一起用
                        if (i == 3)
                        {
                            //ParallelLoopState
                            //取消线程
                            Console.WriteLine("当前线程任务取消");
                            state.Break();
                        }
                        //if (i == 2)
                        //{
                        //    Console.WriteLine("整个循环结束");
                        //    state.Stop();
                        //}
                        string s = c.DoSomethingLong(100, 1000);
                        Console.WriteLine($"{i}   +s");
                    });
                });
            }


            Console.WriteLine(c.Display("ClickOut"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(c.Display("MainThreadDoing"));
            number++;
            label1.Text = number.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            number = 0;
            label1.Text = number.ToString();
        }
    }
}