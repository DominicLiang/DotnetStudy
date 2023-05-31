using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace _08_ASP.NET筛选器.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public DemoController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public string Test1()
        {
            Console.WriteLine("Action执行中");
            return "";
        }

        //[HttpPost]
        //public async Task<string> Test2()
        //{
        //    _dbContext.books.Add(new Book { Name = ".NET书1", Price = 1 });
        //    await _dbContext.SaveChangesAsync();
        //    _dbContext.persons.Add(new Person { Name = "yzk", Age = 18 });
        //    await _dbContext.SaveChangesAsync();
        //    return "OK";
        //}
    }
}
