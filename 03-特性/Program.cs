using System.Reflection;

namespace _03_特性;

// 1 特性attribute，和注释有什么区别 （注释对运行并没有影响）
// 2 声明和使用attribute，AttributeUsage
// 3 运行中获取attribute：额外信息 额外操作
// 4 Remark封装、attribute验证
// 
// 特性：中括号声明
//      就是一个类，直接/间接继承自attribute
//      一般以Attribute结尾，声明时可以省略掉
// 错觉：每一个特性都可以带来对应的功能
// 实际：特性添加后，编译会在元素内部产生IL
//      但是我们是没办法直接使用的，而且在metadata里面会有记录

// 特性本身没有用，就像一个扩充类
// 程序运行的过程中，我们能找到特性，而且也能应用
// 任何一个可以生效的特性，都是因为有地方主动使用了的
//
// 可以在没有破坏类型封装的前提下，可以加点额外的信息和行为

// AttributeUsage特性，用来修饰自己写的自定义特性
[AttributeUsage(AttributeTargets.All,
                AllowMultiple = false,
                Inherited = true)]  // 决定自定义特性是否可以继承 默认true
public class CustomAttribute : Attribute
{
    public CustomAttribute()
    {

    }

    public CustomAttribute(int id)
    {

    }

    public string Description { get; set; } = string.Empty;
    public string Remark = string.Empty;

    public void Show()
    {
        Console.WriteLine($"This is {nameof(CustomAttribute)}");
    }

    // 委托 事件 都可以
}

// 特性的使用
//[Custom]                   // 无参特性
//[Custom()]                 // 无参特性
//[Custom(123), Custom(123)] // 有参特性和重复使用
[Custom(123, Description = "类", Remark = "类")] // 有参特性同时赋值里面的字段或属性
public class Student
{
    [Custom(Description = "属性", Remark = "属性")]
    public int Id { get; set; }
    [Leng(5, 10)]
    public string Name { get; set; } = string.Empty;
    [Long(10001, 999999999999)]
    public long QQ { get; set; }

    [Custom]
    public void Study()
    {
        Console.WriteLine($"这里是{Name}跟着Eleven老师学习");
    }

    [Custom(Description = "方法", Remark = "方法")]
    [return: Custom(Description = "返回值", Remark = "返回值")]// 给返回值加特性
    public string Answer([Custom(Description = "参数", Remark = "参数")] string name)// 给参数加特性也可以
    {
        return $"This is {name}";
    }
}

public class Manager
{
    public static void Show(Student student)
    {
        // 获取类上的特性
        Type type = student.GetType();
        if (type.IsDefined(typeof(CustomAttribute), true))// 检查有没有某个特性
        {
            CustomAttribute? attribute = Attribute.GetCustomAttribute(type, typeof(CustomAttribute)) as CustomAttribute;
            // 获取到特性实例之后，实例里面所有public字段方法都可以调用，就像一个普通对象一样
            Console.WriteLine($"{attribute?.Description}_{attribute?.Remark}");
            attribute?.Show();
        }
        // 获取属性上的特性
        PropertyInfo? property = type.GetProperty("Id");
        if (property != null && property.IsDefined(typeof(CustomAttribute), true))
        {
            CustomAttribute? attribute = Attribute.GetCustomAttribute(property, typeof(CustomAttribute)) as CustomAttribute;
            Console.WriteLine($"{attribute?.Description}_{attribute?.Remark}");
            attribute?.Show();
        }
        // 获取方法上的特性
        MethodInfo? method = type.GetMethod("Answer");
        if (method != null && method.IsDefined(typeof(CustomAttribute), true))
        {
            CustomAttribute? attribute = Attribute.GetCustomAttribute(method, typeof(CustomAttribute)) as CustomAttribute;
            Console.WriteLine($"{attribute?.Description}_{attribute?.Remark}");
            attribute?.Show();
        }
        // 获取方法参数上的特性
        ParameterInfo? parameter = method?.GetParameters()[0];
        if (parameter != null && parameter.IsDefined(typeof(CustomAttribute), true))
        {
            CustomAttribute? attribute = Attribute.GetCustomAttribute(parameter, typeof(CustomAttribute)) as CustomAttribute;
            Console.WriteLine($"{attribute?.Description}_{attribute?.Remark}");
            attribute?.Show();
        }
        // 获取方法返回值上的特性
        ParameterInfo? returnParameter = method?.ReturnParameter;
        if (returnParameter != null && returnParameter.IsDefined(typeof(CustomAttribute), true))
        {
            CustomAttribute? attribute = Attribute.GetCustomAttribute(returnParameter, typeof(CustomAttribute)) as CustomAttribute;
            Console.WriteLine($"{attribute?.Description}_{attribute?.Remark}");
            attribute?.Show();
        }

        Console.WriteLine($"是否正确 {student.Validate()}");

        student.Study();
        string result = student.Answer("Eleven");
        Console.WriteLine(result);
    }
}

// 枚举项也可以使用特性
public enum UserState
{
    [Remark("正常")]
    Normal = 0,
    [Remark("冻结")]
    Frozen = 1,
    [Remark("删除")]
    Deleted = 2
}

public class RemarkAttribute : Attribute
{
    public RemarkAttribute(string remark)
    {
        _remark = remark;
    }

    private string _remark = string.Empty;

    public string GetRemark()
    {
        return _remark;
    }
}

public static class RemarkExtension
{
    public static string GetRemark(this UserState value)
    {
        Type type = value.GetType();
        FieldInfo? field = type.GetField(value.ToString());
        if (field != null && field.IsDefined(typeof(RemarkAttribute), true))
        {
            RemarkAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(RemarkAttribute)) as RemarkAttribute;
            if (attribute != null) return attribute.GetRemark();
        }
        return value.ToString();
    }
}

public abstract class ValidateAttribute : Attribute
{
    public abstract bool Validate(object value);
}

public class LongAttribute : ValidateAttribute
{
    private long _min = 0;
    private long _max = 0;
    public LongAttribute(long min, long max)
    {
        _min = min;
        _max = max;
    }

    public override bool Validate(object value)
    {
        if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
        {
            if (long.TryParse(value.ToString(), out long lResult))
            {
                if (lResult > _min && lResult < _max) return true;
            }
        }
        return false;
    }
}

public class LengAttribute : ValidateAttribute
{
    private long _min = 0;
    private long _max = 0;
    public LengAttribute(long min, long max)
    {
        _min = min;
        _max = max;
    }

    public override bool Validate(object value)
    {
        if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
        {
            if (value.ToString()?.Length >= _min && value.ToString()?.Length <= _max) return true;
        }
        return false;
    }
}

public static class CalidateExtension
{
    public static bool Validate(this object oObject)
    {
        Type type = oObject.GetType();
        foreach (var prop in type.GetProperties())
        {
            if (prop.IsDefined(typeof(ValidateAttribute), true))
            {
                object[] attributes = prop.GetCustomAttributes(typeof(ValidateAttribute), true);
                foreach (ValidateAttribute attribute in attributes)
                {
                    object? o = prop.GetValue(oObject);
                    if (o != null && !attribute.Validate(o))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}

public class Program
{
    static void Main()
    {
        {
            Student student = new Student();
            student.Id = 123;
            student.Name = "Time1";
            student.QQ = 123456;
            Manager.Show(student);
        }
        {
            Console.WriteLine(UserState.Normal.GetRemark());
            Console.WriteLine(UserState.Frozen.GetRemark());
            Console.WriteLine(UserState.Deleted.GetRemark());
        }
    }
}