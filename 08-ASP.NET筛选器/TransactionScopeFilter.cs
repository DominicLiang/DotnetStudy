using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace _08_ASP.NET筛选器;

public class TransactionScopeFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //context.ActionDescriptor中是当前被执行的action方法的描述信息
        //context.ActionArguments中是当前被执行的action方法的参数信息
        ControllerActionDescriptor? cad = context.ActionDescriptor as ControllerActionDescriptor;
        bool isTX = false;
        if (cad != null)
        {
            // ctrlActionDesc.MethodInfo当前action方法
            bool hasNotTransAttribute = cad.MethodInfo.GetCustomAttributes(typeof(NotTransationAttribute), false).Any();
            isTX = !hasNotTransAttribute;
        }

        if (isTX)
        {
            using TransactionScope tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var r = await next();
            if (r.Exception == null)
            {
                tx.Complete();
            }
        }
        else
        {
            await next();
        }
    }
}
