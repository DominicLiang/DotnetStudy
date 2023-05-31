using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _04_ASP.NET中使用依赖注入.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly Calculator _calculator;
    public TestController(Calculator calculator)
    {
        _calculator = calculator;
    }

    [HttpGet]
    public int Add(int i, int j)
    {
        return _calculator.Add(i, j);
    }

    [HttpGet]// 这样可以再使用这个方法的时候才注入构建时间长的服务
    public string DoSomethingLong([FromServices]DoSomethingLong doSomethingLong)
    {
        return doSomethingLong.Display();
    }
}
