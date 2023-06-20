using MediatR;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace _16_MediatR.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;
    private readonly MyDbContext _dbContext;
    private readonly IWebHostEnvironment env;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator, MyDbContext dbContext, IWebHostEnvironment env)
    {
        _logger = logger;
        _mediator = mediator;
        _dbContext = dbContext;
        this.env = env;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        if (env.IsProduction())
        {
            await Console.Out.WriteLineAsync("pro");
        }
        else
        {
            await Console.Out.WriteLineAsync("dev");
        }
        //{
        //    User user = new User("HelloWorld");
        //    _dbContext.Users.Add(user);
        //    await _dbContext.SaveChangesAsync();
        //}
        //{
        //    User user = _dbContext.Users.Single(e => e.Id == 1);
        //    user.ChangeUserName("ÄãºÃ");
        //    await _dbContext.SaveChangesAsync();
        //}


        var res = await _mediator.Send(new WithReturnValue() { msg = "hello" });
        Console.WriteLine(res);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}