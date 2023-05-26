using System.Linq.Expressions;

namespace _09_表达式目录树2;

public class People
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

// ExpressionVisitor:
// 递归解析表达式目录树，因为不知道树的深度
// 只有一个入口 Visit
public class OperationsVisitor : ExpressionVisitor
{
    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.NodeType != ExpressionType.Add) return base.VisitBinary(node);
        return Expression.Subtract(Visit(node.Left), Visit(node.Right));
    }
}

// ORM 把数据库映射到程序内存 通过操作对象来完成对数据库的管理
public class ExpressionVisitorTest
{
    public static void Show()
    {
        {
            Expression<Func<int, int, int>> exp = (m, n) => m * n + 2;
            OperationsVisitor visitor = new OperationsVisitor();
            Expression<Func<int, int, int>> exp2 = (Expression<Func<int, int, int>>)visitor.Visit(exp);
            Console.WriteLine(exp.Compile().Invoke(1, 2));
            Console.WriteLine(exp2.Compile().Invoke(1, 2));
        }
        {
            Expression<Func<People, bool>> lambda = x => x.Age > 5;
            ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
            vistor.Visit(lambda);
            Console.WriteLine(vistor.Condition());
        }
        {
            Expression<Func<People, bool>> lambda = x =>
            x.Age > 5 && x.Id > 5
            && x.Name.StartsWith("1")
            && x.Name.EndsWith("1")
            && x.Name.Contains("1");
            ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
            vistor.Visit(lambda);
            Console.WriteLine(vistor.Condition());
        }
    }
}

public class ConditionBuilderVisitor : ExpressionVisitor
{
    private Stack<string> stringStack = new();
    public string Condition()
    {
        string condition = string.Concat(stringStack.ToArray());
        stringStack.Clear();
        return condition;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node == null) throw new ArgumentNullException("BinaryExpressionNode is null");
        stringStack.Push(")");
        base.Visit(node.Right);
        stringStack.Push($" {node.NodeType.ToSqlOperator()} ");
        base.Visit(node.Left);
        stringStack.Push("(");
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node == null) throw new ArgumentNullException("BinaryExpressionNode is null");
        stringStack.Push($"[{node.Member.Name}]");
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node == null) throw new ArgumentNullException("BinaryExpressionNode is null");
        stringStack.Push($"'{node.Value}'");
        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node == null) throw new ArgumentNullException("BinaryExpressionNode is null");
        string format = string.Empty;
        switch (node.Method.Name)
        {
            case "StartsWith":
                format = "({0} LIKE {1}+'%')";
                break;
            case "Contains":
                format = "({0} LIKE '%'+{1}+'%')";
                break;
            case "EndsWith":
                format = "({0} LIKE '%'+{1})";
                break;
            default:
                break;
        }
        Visit(node.Object);
        Visit(node.Arguments[0]);
        string right = stringStack.Pop();
        string left = stringStack.Pop();
        stringStack.Push(string.Format(format, left, right));

        return node;
    }
}

public static class Extend
{
    public static string ToSqlOperator(this ExpressionType type)
    {
        switch (type)
        {
            case ExpressionType.And:
            case ExpressionType.AndAlso:
                return "AND";
            case ExpressionType.Or:
            case ExpressionType.OrElse:
                return "OR";
            case ExpressionType.Not:
                return "NOT";
            case ExpressionType.NotEqual:
                return "<>";
            case ExpressionType.GreaterThan:
                return ">";
            case ExpressionType.GreaterThanOrEqual:
                return ">=";
            case ExpressionType.LessThan:
                return "<";
            case ExpressionType.LessThanOrEqual:
                return "<=";
            default:
                return string.Empty;
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        ExpressionVisitorTest.Show();
    }
}