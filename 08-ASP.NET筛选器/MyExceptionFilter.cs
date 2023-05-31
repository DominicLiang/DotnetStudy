using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _08_ASP.NET筛选器
{
    public class MyExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _hostEnv;

        public MyExceptionFilter(IWebHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            // context.Exception代表异常信息对象
            // 如果给context.ExceptionHandled复制为true
            // 侧其他ExceptionFilter不会再执行
            string msg;
            if (_hostEnv.IsDevelopment())
            {
                msg = context.Exception.ToString();
            }
            else
            {
                msg = "服务器发生未处理异常";
            }
            ObjectResult objectResult = new(new { code = 500, message = msg });
            context.Result = objectResult;

            //ExceptionHandled设置为true的话，后面的ExceptionFilter不会执行
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
