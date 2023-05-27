using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_异步和多线程;


// 1.进程-线程-多线程，同步与异步
// 2.委托启动异步调用(.net新版已经不再支持 用async await Task替代)
// 3.多线程特点：不卡主线程、速度快、无序性
// 4.异步的或掉和状态参数
// 5.异步等待三种方式
// 6.异步返回值

// 进程：一个程序运行时，占用的全部计算资源的总和
// 线程：程序执行流的最小单位，任何操作都市由线程完成
//      线程是依托于进程存在的，一个进程可以包含多个线程
//      线程也可以有自己的计算资源
// 多线程：多个执行流同时运行

// 同步：完成计算之后，再进入下一行
// 异步：不会等待方法的完成，会直接进入下一行

// 异步方法
// 1.同步方法卡界面,主（UI）线程忙于计算；
//   异步多线程方法不卡节目，主线程完事了，计算任务交给子线程再做；
// 2.同步方法慢，只有一个线程干活
//   异步多线程方法快，因为多个线程并发运算；
//   并不是线性增长，因为资源换时间；
// 3.异步多线程无序：启动无序 执行时间不确定 结束也无序

// Thread：C#语言对线程对象的封装

internal class TestClass
{
    public string DoSomethingLong()
    {
        Console.WriteLine(Display("DoSomethingLong Start"));
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(500);
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