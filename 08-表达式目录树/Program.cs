using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace _08_表达式目录树;

public class ActionFunc
{
    public void Show()
    {
        // Action是一个可以支持16个参数无返回值的泛型委托
        // Func是一个可以支持16个参数和1个返回值的泛型委托
    }
}

public class People
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
public class PeopleCopy
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

// 稍微复杂 但是如果两个对象的字段属性不完全吻合的情况下可以使用特性来匹配
public class ReflectionMapper
{
    public static Out Mapper<In, Out>(In inObject) where Out : new()
    {
        Out outObject = new Out();
        foreach (PropertyInfo outProperty in typeof(Out).GetProperties())
        {
            PropertyInfo inProperty = typeof(In).GetProperty(outProperty.Name)!;
            outProperty.SetValue(outObject, inProperty.GetValue(inObject));
        }
        foreach (FieldInfo outField in typeof(Out).GetFields())
        {
            FieldInfo inField = typeof(In).GetField(outField.Name)!;
            outField.SetValue(outObject, inField.GetValue(inObject));
        }
        return outObject;
    }
}
// 更简单 但是必须两个对象的字段和属性完全吻合 而且性能上比反射更慢
public class SerializeMapper
{
    public static Out Mapper<In, Out>(In inObject) where Out : new()
    {
        string jsonString = JsonSerializer.Serialize(inObject);
        return JsonSerializer.Deserialize<Out>(jsonString)!;
    }
}
// 第一次使用的时候通过反射和表达式目录树动态生成硬编码保存进字典
// 之后再使用直接从字典获取，这样能解决性能问题
public class ExpressionMapper
{
    private static Dictionary<string, object> dic = new Dictionary<string, object>();

    public static Out Mapper<In, Out>(In inObject)
    {
        string key = $"{typeof(In).FullName}-{typeof(Out).FullName}";
        if (dic.ContainsKey(key)) return ((Func<In, Out>)dic[key]).Invoke(inObject);

        ParameterExpression paraExp = Expression.Parameter(typeof(In), "in");

        List<MemberBinding> memberBindings = new List<MemberBinding>();
        foreach (PropertyInfo outProperty in typeof(Out).GetProperties())
        {
            PropertyInfo inProperty = typeof(In).GetProperty(outProperty.Name)!;
            MemberExpression inPropertyExp = Expression.Property(paraExp, inProperty);
            memberBindings.Add(Expression.Bind(outProperty, inPropertyExp));
        }
        foreach (FieldInfo outField in typeof(Out).GetFields())
        {
            FieldInfo inField = typeof(In).GetField(outField.Name)!;
            MemberExpression inFieldExp = Expression.Field(paraExp, inField);
            memberBindings.Add(Expression.Bind(outField, inFieldExp));
        }

        NewExpression newExp = Expression.New(typeof(Out));
        MemberInitExpression bodyExp = Expression.MemberInit(newExp, memberBindings.ToArray());

        Expression<Func<In, Out>> lambda = Expression.Lambda<Func<In, Out>>(bodyExp, paraExp);
        Func<In, Out> func = lambda.Compile();
        dic[key] = func;
        return func.Invoke(inObject);
    }
}

// 泛型缓存
// 根据泛型类型的不同在*静态构造函数*初始化时生成唯一的委托
// 泛型缓存的性能比字典缓存更好
public class ExpressionGenericMapper<In,Out>
{
    private readonly static Func<In, Out> func;

    public static Out Mapper(In inObject)
    {
        return func.Invoke(inObject);
    }

    static ExpressionGenericMapper()
    {
        ParameterExpression paraExp = Expression.Parameter(typeof(In), "in");

        List<MemberBinding> memberBindings = new List<MemberBinding>();
        foreach (PropertyInfo outProperty in typeof(Out).GetProperties())
        {
            PropertyInfo inProperty = typeof(In).GetProperty(outProperty.Name)!;
            MemberExpression inPropertyExp = Expression.Property(paraExp, inProperty);
            memberBindings.Add(Expression.Bind(outProperty, inPropertyExp));
        }
        foreach (FieldInfo outField in typeof(Out).GetFields())
        {
            FieldInfo inField = typeof(In).GetField(outField.Name)!;
            MemberExpression inFieldExp = Expression.Field(paraExp, inField);
            memberBindings.Add(Expression.Bind(outField, inFieldExp));
        }

        NewExpression newExp = Expression.New(typeof(Out));
        MemberInitExpression bodyExp = Expression.MemberInit(newExp, memberBindings.ToArray());

        Expression<Func<In, Out>> lambda = Expression.Lambda<Func<In, Out>>(bodyExp, paraExp);
        func = lambda.Compile();
    }
}

public class ExpressionTest
{
    public static void Show()
    {
        People people = new People()
        {
            Id = 1,
            Name = "123",
            Age = 3
        };
        {
            // 表达式目录树：语法树，或者说是一种数据结构；可以被我们解析
            // 表达式目录树的作用可以理解为动态编码（替代反射）
            Func<int, int, int> func = (m, n) => m * n + 2;
            // 表达式目录树只可以用单行的lambda，多行会报错
            //Expression<Func<int, int, int>> exp2 = (m, n) =>
            //{
            //    return m * n + 2;
            //};

            int res = func.Invoke(12, 23);
        }
        {
            // 快捷的表达式目录数声明方式
            Expression<Func<int, int, int>> exp = (m, n) => m * n + 2;
            int res1 = exp.Compile().Invoke(12, 23);
            // 自己拼装表达式目录树
            ParameterExpression paraL = Expression.Parameter(typeof(int), "l");
            ParameterExpression paraR = Expression.Parameter(typeof(int), "r");
            BinaryExpression binaryL = Expression.Multiply(paraL, paraR);
            ConstantExpression conR = Expression.Constant(2);
            BinaryExpression binary = Expression.Add(binaryL, conR);
            Expression<Func<int, int, int>> actExpression = Expression.Lambda<Func<int, int, int>>(binary, paraL, paraR);
            int res2 = actExpression.Compile().Invoke(12, 23);
        }
        {
            PeopleCopy peopleCopy = ReflectionMapper.Mapper<People, PeopleCopy>(people);
        }
        {
            PeopleCopy peopleCopy = SerializeMapper.Mapper<People, PeopleCopy>(people);
        }
        {
            PeopleCopy peopleCopy = ExpressionMapper.Mapper<People, PeopleCopy>(people);
        }
        {
            PeopleCopy peopleCopy = ExpressionGenericMapper<People,PeopleCopy>.Mapper(people);
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        ExpressionTest.Show();
    }
}