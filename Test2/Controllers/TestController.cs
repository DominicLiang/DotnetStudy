using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test2.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpPost]
    public IActionResult Test(DataViewModel data)
    {
        //throw new NullReferenceException("null");
        return Ok(new Response("200", "111111111111111111111", data));
        //return Ok();
    }
}
