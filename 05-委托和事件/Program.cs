using System;

namespace _05_委托和事件;

// 1.委托的声明、实例化和调用
// 2.泛型委托  Action  Func
// 3.委托的意义： 解耦  异步多线程  多播委托
// 4.事件，观察者模式
// 委托：是一个类，继承自System.MulticastDelegate,里面内置了几个方法
public delegate void NoReturnNoParaOutClass();

public class Boss
{
    public delegate void Reception();
    // 事件：事件是带了event的委托实例
    //      event限制了变量被外部调用或直接赋值，子类也不可以
    //      可以通过一个事件去开放接口，让外部可以随意扩展动作
    // 委托和事件的区别与联系
    // 委托是一个类，事件是委托的实例
    // 事件：可以把一堆可变的动作/行为封装出去，交给第三方来指定
    //      预定义一样，程序设计的时候，我们可以把程序分成两部分
    //      一部分是固定的，直接写死；还有不固定的地方，通过一个
    //      事件去开放接口，外部可以随意扩展
    // 框架：完成固定/通用部分，把可变部分留出扩展点，支持自定义
    public event Reception? reception;

    public void ComeBack()
    {
        Console.WriteLine("老板回来了");
        Console.WriteLine("前台通知各员工");
        reception?.Invoke();
    }
}
public class Staff1
{
    public void CloseNBA()
    {
        Console.WriteLine("关闭NBA直播，继续工作！");
    }
}
public class Staff2
{
    public void CloseStock()
    {
        Console.WriteLine("关闭股票行情，继续工作！");
    }
}

public class MyDelegate
{
    // 1.声明委托
    public delegate void NoReturnNoPara();
    public delegate void NoReturnWithPara(int x, int y);
    public delegate int WithReturnNoPara();
    public delegate string WithReturnWithPara(out int x, ref int y);

    public void Show()
    {
        Student student = new Student()
        {
            Id = 123,
            Name = "靠谱一大叔",
            Age = 32,
            ClassId = 1,
        };
        student.Study();
        {
            // 把方法包装成变量，invoke的时候自动执行方法
            // 2.委托的实例化
            NoReturnNoPara noReturnNoPara1 = new NoReturnNoPara(DoNothing);
            NoReturnNoPara noReturnNoPara2 = DoNothing;
            // 3.委托实例的调用
            noReturnNoPara1.Invoke();
            noReturnNoPara2();
        }
        {
            // BeginInvoke
            // EndInvoke
            // 异步多线程
            WithReturnNoPara withReturnNoPara1 = new WithReturnNoPara(GetSomething1);
            WithReturnNoPara withReturnNoPara2 = GetSomething1;
            int result = withReturnNoPara1.Invoke();
            //IAsyncResult asyncResult = withReturnNoPara2.BeginInvoke(null, null);
            //withReturnNoPara2.EndInvoke(asyncResult);
        }
        {
            // 多播委托
            // +=为委托实例按顺序增加方法，形成方法链，Invoke时按顺序执行
            NoReturnNoPara noReturnNoPara = DoNothing;
            noReturnNoPara += DoNothingStatic;
            noReturnNoPara += Student.StudyAdvanced;
            // -=为委托实例移除方法，从方法链的尾部开始匹配，一次一个，没有也不异常
            noReturnNoPara -= DoNothingStatic;
            noReturnNoPara -= Student.StudyAdvanced;
        }
        {
            WithReturnNoPara withReturnNoPara = GetSomething1;
            withReturnNoPara += GetSomething2;
            withReturnNoPara += GetSomething3;
            int result = withReturnNoPara.Invoke();
            // 此处返回3
            // 多播委托带返回值，结果是最后加入的方法的返回值
        }
    }

    private int GetSomething1()
    {
        return 1;
    }
    private int GetSomething2()
    {
        return 2;
    }
    private int GetSomething3()
    {
        return 3;
    }

    private void DoNothing()
    {
        Console.WriteLine("This is DoNothing");
    }
    private static void DoNothingStatic()
    {
        Console.WriteLine("This is DoNothingStatic");
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public int Age { get; set; }
    public void Study()
    {
        Console.WriteLine("学习.net");
    }
    public static void StudyAdvanced()
    {
        Console.WriteLine("学习Vip");
    }
    public static void Show()
    {
        Console.WriteLine("Show");
    }
}

public class ListExtend
{
    private List<Student> GetStudentList()
    {
        List<Student> studentList = new List<Student>()
        {
            new Student() { Id = 1, Name="老K",ClassId=2,Age=35} ,
            new Student() { Id = 2, Name="hao",ClassId=2,Age=23} ,
            new Student() { Id = 3, Name="大水",ClassId=2,Age=27} ,
            new Student() { Id = 4, Name="半醉人间",ClassId=2,Age=26} ,
            new Student() { Id = 5, Name="风尘浪子",ClassId=2,Age=25} ,
            new Student() { Id = 6, Name="一大锅鱼",ClassId=2,Age=24} ,
            new Student() { Id = 7, Name="小白",ClassId=2,Age=21}
        };

        return studentList;
    }

    public void Show()
    {
        List<Student> students = GetStudentList();
        {
            // 找出年龄大于25
            List<Student> result = new List<Student>();
            //foreach (var student in students)
            //{
            //    if (student.Age > 25)
            //    {
            //        result.Add(student);
            //    }
            //}

            result = GetList(students, Than);
            Console.WriteLine(result.Count);
        }
    }

    // 委托解耦，减少重复代码
    private List<Student> GetList(List<Student> students, ThanDelegate than)
    {
        List<Student> result = new List<Student>();
        foreach (var student in students)
        {
            if (than.Invoke(student))
            {
                result.Add(student);
            }
        }
        return result;
    }

    public delegate bool ThanDelegate(Student student);

    private bool Than(Student student)
    {
        return student.Age > 25;
    }
}

class Program
{
    static void Main()
    {
        MyDelegate myDelegate = new MyDelegate();
        myDelegate.Show();

        ListExtend listExtend = new ListExtend();
        listExtend.Show();

        Boss boss = new Boss();
        boss.reception += new Staff1().CloseNBA;
        boss.reception += new Staff2().CloseStock;
        boss.ComeBack();
    }
}