using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11__异步和多线程的发展;

internal class Class1
{
    public string DoSomethingLong(int min = 500, int max = 500)
    {
        Console.WriteLine(Display("DoSomethingLong Start"));
        for (int i = 0; i < 10; i++)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int time = random.Next(min, max);
            Thread.Sleep(time);
            Console.WriteLine(Display("SomethingLongDoing"));
        }

        return Display("Finish");
    }

    public void DoOtherthing()
    {
        Console.WriteLine(Display("DoOtherthing Start"));
        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(200);
            Console.WriteLine(Display("OtherthingDoing"));
        }
        Console.WriteLine(Display("DoOtherthing Finish"));
    }

    public string Display(string title)
    {
        return $"{title}  ThreadID:{Thread.CurrentThread.ManagedThreadId}  Time:{DateTime.Now.ToString("mm-ss-ff")}";
    }
}