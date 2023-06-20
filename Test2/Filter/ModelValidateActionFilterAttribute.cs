using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Test2.Filter;

public class ModelValidateActionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            //获取验证失败的模型字段
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => e.Value.Errors.First().ErrorMessage)
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

            context.Result = new BadRequestObjectResult(result);
        }

    }
}
