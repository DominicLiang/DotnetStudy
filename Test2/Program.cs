var builder = WebApplication.CreateBuilder(args);

var s = builder.Configuration.GetSection("test");
Console.WriteLine(s);
Console.WriteLine(s["Id"]);
Console.WriteLine(s["UserName"]);
Console.WriteLine(s["Password"]);
var ss = s.Get<JWT>();
Console.WriteLine(ss.Id + "   " + ss.UserName + "   " + ss.Password);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class JWT
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}