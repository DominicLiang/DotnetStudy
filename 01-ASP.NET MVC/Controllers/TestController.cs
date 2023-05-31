using _01_ASP.NET_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace _01_ASP.NET_MVC.Controllersl;

public class TestController : Controller
{
    // Action方法：操作方法
    // 视图名需要对应这个方法名 不区分大小写
    public IActionResult Demo1()
    {
        var model = new Person("Test", true, DateTime.Now);
        return View(model);
    }
}