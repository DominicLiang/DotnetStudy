using Microsoft.AspNetCore.Mvc.Filters;

namespace _08_ASP.NET筛选器
{
    public class MyActionFilter1 : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("MyActionFilter1 前代码");
            ActionExecutedContext result = await next();
            if(result.Exception!=null)
            {
                Console.WriteLine("MyActionFilter1：发生异常");
            }
            else
            {
                Console.WriteLine("MyActionFilter1:执行成功");
            }
        }
    }
}
