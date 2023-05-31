using _08_ASP.NET筛选器;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

// 注意Filter的执行顺序是从下往上的
builder.Services.Configure<MvcOptions>(opt =>
{
    //opt.Filters.Add<LogExceptionFilter>();
    //opt.Filters.Add<MyExceptionFilter>();

    //opt.Filters.Add<MyActionFilter1>();
    //opt.Filters.Add<MyActionFilter2>();

    //opt.Filters.Add<TransactionScopeFilter>();

    opt.Filters.Add<RateLimitActionFilter>();

});

builder.Services.AddDbContext<MyDbContext>(opt =>
{
    var folder = Environment.SpecialFolder.DesktopDirectory;
    string path = Environment.GetFolderPath(folder);
    string DbPath = Path.Join(path, "TestDB.db");
    opt.UseSqlite($"Data Source={DbPath}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
