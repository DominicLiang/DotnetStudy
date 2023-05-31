using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _02_WebAPI入门.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public Person GetPerson()
    {
        return new Person(1, "YZK", 18);
    }

    [HttpPost]
    public string[] SaveNote(SaveNoteRequest request)
    {
        System.IO.File.WriteAllText(request.Title + ".txt", request.Content);
        return new string[] { "ok", request.Title };
    }
}
