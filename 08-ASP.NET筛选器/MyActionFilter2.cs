using Microsoft.AspNetCore.Mvc.Filters;

namespace _08_ASP.NET筛选器
{
    public class MyActionFilter2 : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("MyActionFilter2 前代码");
            ActionExecutedContext result = await next();
            if (result.Exception != null)
            {
                Console.WriteLine("MyActionFilter2：发生异常");
            }
            else
            {
                Console.WriteLine("MyActionFilter2:执行成功");
            }
        }
    }
}
