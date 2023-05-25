using System.Reflection;
using System;
using System.Configuration;

namespace _02_反射;


// 1 dll-IL-metadata-反射
// 2 反射加载dll，读取module 类 方法 特性
// 3 反射创建对象，反射+简单工厂+配置文件 选修：破坏单例 创建泛型
// 4 反射调用实例方法 静态方法 重载方法 选修：调用私有方法 调用泛型方法
// 5 反射字段和属性，分别获取值和设置值
// 6 反射的好处和局限
//
// 反射：System.Reflection .Net框架提供的帮助类库，可以读取并是使用metadata
// 反射优点: 动态
// 反射缺点: 1.写起来复杂
//                 2.避开编译器的检查
//                 3.性能问题 但是绝对值小 绝大部分情况不影响你的程序性能

public abstract class Test
{
    public abstract void SayHi();
}

public class Test1 : Test
{
    public override void SayHi()
    {
        Console.WriteLine("Test1: Hello World");
    }

    private void Show1(string s)
    {
        Console.WriteLine(s);
    }

    public static void Show2(string s)
    {
        Console.WriteLine(s);
    }

    public void Show3(int i)
    {
        Console.WriteLine(i);
    }

    public void Show3(string s)
    {
        Console.WriteLine(s);
    }

    public void Show3(string s, int i)
    {
        Console.WriteLine(s + i.ToString());
    }
}

public class Test2 : Test
{
    public override void SayHi()
    {
        Console.WriteLine("Test2: Hello World");
    }
}

public class Factory
{
    private static string testConfig = ConfigurationManager.AppSettings["TestConfig"];

    public static Test? CreateTest()
    {
        Assembly assembly = Assembly.Load("2-反射");
        Type? type = assembly.GetType(testConfig);
        Test? test = (Test?)Activator.CreateInstance(type); // 无参数创建实例
        // Test? test2 = (Test?)Activator.CreateInstance(type, true); // 使用私有构造函数创建实例
        // Test? test3 = (Test?)Activator.CreateInstance(type, new object[] { }); // 有参数创建实例
        return test;
    }
}

public class GenericDouble<T>
{
    public void Show<W, X>(T t, W w, X x)
    {
        Console.WriteLine($"t.type={t?.GetType()}, w.type={w?.GetType()}, x.type={x?.GetType()}");
    }
}

public class People
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Id2;
    public string Name2;
    public string Description2;
}

public class PeopleDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class Program
{
    static void Main()
    {
        // 加载dll
        Assembly assembly1 = Assembly.Load("2-反射");// dll名称无后缀，在exe同目录加载
        // Assembly assembly2 =Assembly.LoadFile(@"D:\...\2-反射.dll");// 需要完整路径 使用时必须要有依赖项
        // Assembly assembly3 =Assembly.LoadFrom("2-反射.dll");// 同目录带后缀 或者 完整路径

        Module[] modules = assembly1.GetModules();
        foreach (var item in modules)
        {
            Console.WriteLine(item.FullyQualifiedName);
        }

        Type[] types = assembly1.GetTypes();
        foreach (var item in types)
        {
            Console.WriteLine(item.FullName);
        }

        // 反射创建类
        Type? type = assembly1.GetType("Reflection.Test1");
        Test? test = (Test?)Activator.CreateInstance(type);

        foreach (var item in type.GetMethods())
        {
            Console.WriteLine(item.Name);
        }

        // 程序的可配置可扩展
        // 利用反射可以通过配置文件来动态加载dll和使用
        Test? test2 = Factory.CreateTest();
        test2?.SayHi();

        // 反射使用方法
        // methodInfo.Invoke来使用方法
        // 参数一为使用此方法的实例
        // 参数二为使用此方法需要传入的参数
        MethodInfo? method1 = type.GetMethod("SayHi");
        method1?.Invoke(test, null);// 无参数方法参数可以填null

        MethodInfo? method2 = type.GetMethod("Show1", BindingFlags.Instance | BindingFlags.NonPublic);// 私有方法需要加BindingFlags.Instance | BindingFlags.NonPublic
        method2?.Invoke(test, new object[] { "我是私有方法" });

        MethodInfo? method3 = type.GetMethod("Show2");
        method3?.Invoke(null, new object[] { "我是静态方法" });// 静态方法的实例可以填null

        // 多个重载的方法可以在GetMethod的时候通过参数类型来选择
        MethodInfo? method4 = type.GetMethod("Show3", new Type[] { typeof(int) });
        method4?.Invoke(test, new object[] { 123 });

        MethodInfo? method5 = type.GetMethod("Show3", new Type[] { typeof(string) });
        method5?.Invoke(test, new object[] { "Hello" });

        MethodInfo? method6 = type.GetMethod("Show3", new Type[] { typeof(string), typeof(int) });
        method6?.Invoke(test, new object[] { "Hello", 123 });

        // 反射创建泛型类
        // GenericDouble`1这个`1表示占位符个数,如果两个占位符就是GenericDouble`2
        // 泛型类需要多一个MakeGenericType的步骤,这一步用来确定泛型的真正类型
        Type? typeG = assembly1?.GetType("Reflection.GenericDouble`1");
        Type? newType = typeG?.MakeGenericType(new Type[] { typeof(int) });
        object? oGeneric = Activator.CreateInstance(newType);

        // 反射使用泛型方法
        // 泛型方法需要多出MakeGenericMethod步骤来确定泛型的类型
        MethodInfo? method = newType.GetMethod("Show");
        MethodInfo? methodNew = method?.MakeGenericMethod(new Type[] { typeof(string), typeof(DateTime) });
        methodNew?.Invoke(oGeneric, new object[] { 123, "Hello", DateTime.Now });

        // 通过反射构造函数来创建实例
        // 字典存放参数的类型和实例
        Dictionary<Type, object> dic = new Dictionary<Type, object>();
        object result;
        // 通过类型反射获得构造函数需要的参数列表
        ConstructorInfo c = type.GetConstructors()[0];// 获取所有构造函数的集合,通过BindingFlags可以获得私有构造函数,下同
        // ConstructorInfo pc = type.GetConstructor(new[] { typeof(string) }); 可以通过传入参数类型来定位多个重载的特定构造函数
        ParameterInfo[] pi = c.GetParameters();
        // 列表为0即无参构造函数,可直接调用invoke来获得实例
        if (pi.Length <= 0) result = c.Invoke(null);
        // 构造函数有参数的话可以用object数组传参数
        // ParameterInfo里面的Position提供数组索引,ParameterType提供参数类型
        object[] objs = new object[pi.Length];
        foreach (ParameterInfo p in pi)
            objs[p.Position] = dic[p.ParameterType];
        result = c.Invoke(objs);
        // 获得私有构造函数
        // ConstructorInfo[] ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

        People people = new People();
        people.Id = 123;
        people.Name = "Hello";
        people.Description = "Hello World";
        Type typePeople = typeof(People);
        // 反射属性 type.GetProperties() 返回PropertyInfo数组(属性数组)
        foreach (PropertyInfo item in typePeople.GetProperties())
        {
            Console.WriteLine(item.Name);
            // 反射修改属性的值
            switch (item.Name)
            {
                case "Id":
                    item.SetValue(people, 234);
                    break;
                case "Name":
                    item.SetValue(people, "Test");
                    break;
                case "Description":
                    item.SetValue(people, "All OK!");
                    break;
                default:
                    break;
            }
            // 反射获取属性的值
            Console.WriteLine(item.GetValue(people));
        }

        foreach (FieldInfo item in typePeople.GetFields())
        {
            Console.WriteLine(item.Name);
            // 反射修改字段的值
            switch (item.Name)
            {
                case "Id2":
                    item.SetValue(people, 345);
                    break;
                case "Name2":
                    item.SetValue(people, "Test2");
                    break;
                case "Description2":
                    item.SetValue(people, "All OK2!");
                    break;
                default:
                    break;
            }
            // 反射获取字段的值
            Console.WriteLine(item.GetValue(people));
        }

        // 两个属性相同的类的复制
        PeopleDTO peopleDTO = new PeopleDTO();
        foreach (PropertyInfo item in typeof(PeopleDTO).GetProperties())
        {
            item.SetValue(peopleDTO, typeof(People).GetProperty(item.Name)?.GetValue(people));
            Console.WriteLine(item.GetValue(peopleDTO));
        }
    }
}