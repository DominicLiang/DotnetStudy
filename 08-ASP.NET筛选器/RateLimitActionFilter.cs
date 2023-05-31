using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace _08_ASP.NET筛选器
{
    public class RateLimitActionFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache _cache;

        public RateLimitActionFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //获取请求的IP地址
            string ip = context.HttpContext.Connection.RemoteIpAddress?.ToString()!;

            string cacheKey = $"last_visit_tick_{ip}";
            long? lastVisit = _cache.Get<long?>(cacheKey);
            //Environment.TickCount64 获得当前时间
            if (lastVisit == null || Environment.TickCount64 - lastVisit > 1000)
            {
                _cache.Set(cacheKey, Environment.TickCount64, TimeSpan.FromSeconds(10));//避免长期不访问的用户，占据缓存的内存
                await next();
            }
            else
            { 
                ObjectResult resutl = new ObjectResult("访问太频繁") { StatusCode = 429 };
                context.Result = resutl;
            }
        }
    }
}
