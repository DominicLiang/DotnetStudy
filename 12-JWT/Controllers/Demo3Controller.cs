using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _12_JWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]//标注这个就会做登录校验
    public class Demo3Controller : ControllerBase
    {
        [HttpGet]
        public string Test1()
        {
            var claim = User.FindFirst(ClaimTypes.Name);
            return "ok " + claim?.Value;
        }

        [Authorize(Roles = "admin")]//只允许管理员查看
        [HttpGet]
        public string Test2()
        {
            return "123";
        }
    }
}
