using EFCoreBooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public string Demo1()
        {
            int c = _dbCtx.Books.Count();
            return c.ToString();
        }
    }
}
