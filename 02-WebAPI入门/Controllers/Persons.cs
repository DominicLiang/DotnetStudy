using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _02_WebAPI入门.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly ILogger<PersonsController> logger;

    public PersonsController(ILogger<PersonsController> logger)
        => this.logger = logger;



    [HttpGet]
    public Person[] GetAll()
    {
        return new Person[]
        {
            new Person(1,"zyk",18),
            new Person(2,"lhr",15),
        };
    }

    [HttpGet]
    public Person? GetById(int id) => id switch
    {
        1 => new Person(1, "yzk", 18),
        2 => new Person(2, "lhr", 15),
        _ => null,
    };

    [HttpPost]
    public string AddNew(Person p)
    {
        return "新增完成";
    }

    [HttpGet]
    public IActionResult Get(int id) => id switch
    {
        1 => Ok(88),
        2 => Ok(99),
        _ => NotFound("id错误")
    };

    [HttpGet]
    public ActionResult<int> Get2(int id) => id switch
    {
        1 => 88,
        2 => 99,
        _ => NotFound("id错误") //返回404
    };

    //捕捉URL占位符
    [HttpGet("Row/{i1}/Column/{i2}")]
    public int Multi(int i1,int i2)
    {
        return i1 + i2;
    }
    [HttpPost]
    public Person Grow(Person p ,int i)
    {
        return new Person(p.Id, p.Name, p.Age + i);
    }
}
