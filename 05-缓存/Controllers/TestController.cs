using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace _05_缓存.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<TestController> _logger;
    private readonly IDistributedCache _distCache;

    public TestController(
        IMemoryCache memoryCache,
        ILogger<TestController> logger,
        IDistributedCache distCache) =>
        (_memoryCache, _logger, _distCache) =
        (memoryCache, logger, distCache);

    //内存缓存
    [HttpGet]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        //GetOrCreateAsync 推荐使用
        //1.从缓存取数据
        //2.如果没有缓存的话，从数据源取数据，返回调用者并保存到缓存
        //* 缓存保存的key值一定要全局唯一
        Book? result = await _memoryCache.GetOrCreateAsync($"book{id}", async (e) =>
        {
            _logger.LogDebug($"缓存里没找到，到数据库中查找 {id}");

            // 手动修改缓存
            //e.SetValue(id);

            // 两种过期时间可以同过被使用

            // 绝对缓存时间
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            // 滑动缓存时间
            e.SlidingExpiration = TimeSpan.FromSeconds(10);

            // 缓存雪崩
            // 1.缓存项集中过期引缓存雪崩
            // 2.解决方案：在基础过期时间之上，再加一个随机的过期时间
            int n = Random.Shared.Next(10, 15);
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(n);

            // IEnumerable陷阱
            // IQueryable和IEnumerable等类型可能存在延迟加载问题
            // 缓存应该禁止这两种类型而强制使用List或数组等类型

            // 分布式内存缓存 除非必要没必要使用 只有在服务器太多的情况下使用
            // 使用缓存服务器 Redis、Memcached等
            // .NET Core中提供了统一的分布式缓存服务器的操作接口IDistributedCahe
            // 用法和内存缓存类似
            // 分布式缓存和内存缓存的区别：缓存值类型为byte[]，需要我们进行类型转换
            // 分布式缓存用法
            // Redis服务器开启方法：Redis安装文件夹Cmd redis-cli
            // Microsoft.Extensions.Caching.StackExchangeRedis 安装包

            return await MyDbContext.GetByIdAsync(id);
        });
        _logger.LogDebug($"GetOrCreateAsync结果{result}");
        if (result == null)
        {
            return NotFound($"找不到id={id}的书");
        }
        else
        {
            return result;
        }
    }

    [HttpGet]
    public async Task<ActionResult<Book>> DistCache(int id)
    {
        // 分布式内存缓存 除非必要没必要使用 只有在服务器太多的情况下使用
        // 使用缓存服务器 Redis、Memcached等
        // .NET Core中提供了统一的分布式缓存服务器的操作接口IDistributedCahe
        // 用法和内存缓存类似
        // 分布式缓存和内存缓存的区别：缓存值类型为byte[]，需要我们进行类型转换
        // 分布式缓存用法
        // Redis服务器开启方法：Redis安装文件夹Cmd redis-cli
        // Microsoft.Extensions.Caching.StackExchangeRedis 安装包
        Book? book;
        string? s = await _distCache.GetStringAsync($"book{id}");
        if (s == null)
        {
            Console.WriteLine("从数据库中取");
            book = await MyDbContext.GetByIdAsync(id);

            // 分布式缓存使用缓存时间
            var opt = new DistributedCacheEntryOptions();
            int n = Random.Shared.Next(10, 15);
            opt.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(n);
            opt.SlidingExpiration = TimeSpan.FromSeconds(10);

            await _distCache.SetStringAsync($"book{id}", JsonSerializer.Serialize(book), opt);
        }
        else
        {
            Console.WriteLine("从缓存中取");
            book = JsonSerializer.Deserialize<Book>(s);
        }
        if (book == null)
        {
            return NotFound("不存在");
        }
        else
        {
            return book;
        }
    }

    //客户端相应缓存：指定在浏览器中缓存多久 单位秒 
    [ResponseCache(Duration = 20)]
    [HttpGet]
    public DateTime Now()
    {
        return DateTime.Now;
    }
}
