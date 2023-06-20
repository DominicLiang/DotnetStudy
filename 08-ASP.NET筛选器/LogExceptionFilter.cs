using Microsoft.AspNetCore.Mvc.Filters;

namespace _08_ASP.NET筛选器
{
    public class LogExceptionFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {

            await File.AppendAllTextAsync("error.log", context.Exception.ToString());
        }
    }
}
