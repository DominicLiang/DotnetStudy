using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Test2.Filter;

public class CustomFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            //获取验证失败的模型字段
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => e.Value?.Errors.First().ErrorMessage)
                .ToList();


            // | 简单组合一下多处错误
            var str = string.Join("|", errors);

            //设置返回内容
            var result = new
            {
                success = false,
                code = 20000,
                msg = str,
                data = ""
            };

            context.Result = new ObjectResult(result);
        }
        else
        {
            await next();
        }
    }
}
