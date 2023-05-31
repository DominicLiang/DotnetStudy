using Microsoft.AspNetCore.Mvc;

namespace _02_WebAPI入门.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HelloController
    {
        [HttpGet]
        public int Add(int i,int j)
        {
            return i + j;
        }

        [HttpGet]
        public async Task<string> GetText()
        {
            string s = await File.ReadAllTextAsync("Hello.txt");
            return s;
        }
    }
}
