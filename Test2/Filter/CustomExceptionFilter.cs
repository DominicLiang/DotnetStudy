using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Test2.Filter;

public class CustomExceptionFilter : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}
