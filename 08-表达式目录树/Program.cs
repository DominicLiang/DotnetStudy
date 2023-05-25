using System.Linq.Expressions;

namespace _08_表达式目录树;

public class ActionFunc
{
    public void Show()
    {
        // Action是一个可以支持16个参数无返回值的泛型委托
        // Func是一个可以支持16个参数和1个返回值的泛型委托
    }
}

public class ExpressionTest
{
    public static void Show()
    {
        {
            // 表达式目录树：语法树，或者说是一种数据结构；可以被我们解析
            Func<int, int, int> func = (m, n) => m * n + 2;
            Expression<Func<int, int, int>> exp = (m, n) => m * n + 2;
            
            // 表达式目录树只可以用单行的lambda，多行会报错
            //Expression<Func<int, int, int>> exp2 = (m, n) =>
            //{
            //    return m * n + 2;
            //};

            int res1 = func.Invoke(12, 23);
            int res2 = exp.Compile().Invoke(12, 23);
        }
        {

        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}