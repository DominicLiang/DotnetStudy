using Apollo;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;
using LogLevel = Com.Ctrip.Framework.Apollo.Logging.LogLevel;

//YamlConfigAdapter.Register();
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((c, b) =>
{
    //LogManager.UseConsoleLogging(LogLevel.Debug);
    b.AddApollo(builder.Configuration);
});
//Console.WriteLine("t1   " + builder.Configuration.GetValue<string>("timeout"));
Console.WriteLine("t2   " + builder.Configuration.GetSection("Authentication.Token").Value);

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
