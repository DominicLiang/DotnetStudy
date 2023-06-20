using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Test2;
using Test2.Filter;

var builder = WebApplication.CreateBuilder(args);

//var s = builder.Configuration.GetSection("test");
//Console.WriteLine(s);
//Console.WriteLine(s["Id"]);
//Console.WriteLine(s["UserName"]);
//Console.WriteLine(s["Password"]);
//var ss = s.Get<JWT>();
//Console.WriteLine(ss.Id + "   " + ss.UserName + "   " + ss.Password);

builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

// Add services to the container.


//builder.Services.Configure<ApiBehaviorOptions>(opt =>
//{
//    opt.InvalidModelStateResponseFactory = actionContext =>
//    {
//        Console.WriteLine("111111111111111111111111111111");
//        //获取验证失败的模型字段 
//        var errors = actionContext.ModelState
//        .Where(e => e.Value.Errors.Count > 0)
//        .Select(e => e.Value.Errors.First().ErrorMessage)
//        .ToList();
//        var strErrorMsg = string.Join("|", errors);
//        //设置返回内容
//        var result = new Response<List<string>>("200", "errors", errors);
//        return new BadRequestObjectResult(result);
//    };
//});

builder.Services.AddControllers(opt =>
{
    //opt.Filters.Add<ModelValidateActionFilterAttribute>();
    opt.Filters.Add<CustomExceptionFilter>();
    opt.Filters.Add<CustomFilter>();

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//public class JWT
//{
//    public string Id { get; set; }
//    public string UserName { get; set; }
//    public string Password { get; set; }
//}