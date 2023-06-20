using EFCoreBooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _07_分层项目中EFCore用法.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Test1Controller : ControllerBase
    {
        private readonly MyDbContext _dbCtx;

        public Test1Controller(MyDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public void T0()
        {

            _dbCtx.Books.Add(new Book { Url = "http://blogs.msdn.com/adonet" });

            _dbCtx.SaveChangesAsync();
        }

        [HttpGet]
        public IActionResult T1()
        {
            var blog = _dbCtx.Books.OrderBy(b => b.Id).First();
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.People = new People { Title = "Hello World", Content = "I wrote an app using EF Core!" };
            _dbCtx.SaveChanges();
            return Ok(blog);
        }

        [HttpGet]
        public IActionResult T2()
        {
            var book = _dbCtx.Books.Include(r => r.People).FirstOrDefault();
            Console.WriteLine(book.People);
            return Ok(book.People);
        }

    }
}
