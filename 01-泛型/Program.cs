using System;

namespace _01_泛型;


// 1 引入泛型：延迟声明 .net2.0
// 2 如果声明和使用泛型
// 3 泛型的好处和原理
//   能使同一个方法能使用不同类型的参数或返回值
//   多用于为不同类型的对象做相同的处理
//   例子: List<>
// 4 泛型类 泛型方法 泛型接口 泛型委托
// 5 泛型约束
// 6 协变 逆变
// 7 泛型缓存
interface ISports
{
    void PlayPingpong();
}

public class People
{
    public int id;
    public string name = String.Empty;
}

public class Chinese : People, ISports
{
    public void PlayPingpong()
    {
        System.Console.WriteLine("Playing Pingpong...");
    }
}

// 泛型类 泛型可以用在所有地方上
// 每个不同的T，都会生成一份不同的副本
// 适合不同的类型，需要缓存一份数据的场景，效率高
public class Test<K>
{
    public K? k;

    // 泛型方法 泛型可以用在参数和返回值上
    public static T Get<T>(T t)
    // where T : People    基类约束（不能是密封类）
    //                     可以使用基类的字段和方法

    // where T : ISports   接口约束
    //                     可以使用接口的字段和方法

    // where T : class     引用类型约束
    //                     可以使用T tNew = null来赋予初始值

    // where T : struct    值类型约束
    //                     可以使用T tNew = default(T)来赋予初始值

    // where T : new()     无参数构造函数约束
    //                     将范围约束到无参构造函数
    //                     这样可以使用T tNew = new T()来创建实例

    // 约束可以多个叠加比如where T : People, ISports, new()，同时约束为People的子类也实现了ISports接口同时还是无参数构造函数
    {
        return t;
    }
}

// 协变
public interface ITest1<out T>
{
    T? Get();
}

class Test12<T> : ITest1<T>
{
    public T? Get()
    {
        return default(T);
    }
}

// 逆变
public interface ITest2<in T>
{
    void Show(T t);
}

public class Test22<T> : ITest2<T>
{
    public void Show(T t)
    {
        System.Console.WriteLine(t);
    }
}

public class GenericCache<T>
{
    static GenericCache()
    {
        System.Console.WriteLine("这是静态构造函数");
        _TypeTime = string.Format($"{typeof(T).FullName}_{DateTime.Now.ToString()}");
    }

    private static string _TypeTime = "";

    public static string GetCache()
    {
        return _TypeTime;
    }
}

public class Program
{
    static void Main()
    {
        // 协变和逆变只能用在接口和委托上
        // 写了out的类型只能作为返回值，写了in的类型只能作为参数
        // 协变的情况下,使用了out的泛型父类可以用子类来new
        // 逆变的情况下,使用了in的泛型子类可以用父类来new

        // 协变
        ITest1<People> people1 = new Test12<Chinese>();
        Func<People> func = new Func<Chinese>(() => new Chinese());
        // 逆变
        ITest2<Chinese> people2 = new Test22<People>();
        Action<Chinese> action = new Action<People>(i => { });

        for (int i = 0; i < 3; i++)
        {
            System.Console.WriteLine(GenericCache<int>.GetCache());
            Thread.Sleep(10);
            System.Console.WriteLine(GenericCache<long>.GetCache());
            Thread.Sleep(10);
        }
    }
}