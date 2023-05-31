namespace _04_ASP.NET中使用依赖注入;

public class DoSomethingLong
{
    public DoSomethingLong()
    {
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(500);
        }
    }

    public string Display()
    {
        return "Hello";
    }
}
