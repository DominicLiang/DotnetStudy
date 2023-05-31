using EFCoreBooks;
using Microsoft.EntityFrameworkCore;
// 官方包 装到类库里
// Microsoft.EntityFrameworkCore.Relational

// EFCore配置
// 1.建类库项目，放实体类、DbContext、配置类等
//   DbContext中不配置数据库连接，而是为DbContext写一个构造函数
//   里面带DbContextOptions类型的参数如：
//   (DbContextOptions<MyDbContext> options) : base(options)
// 2.EFCore项目安装：Microsoft.EntityFrameworkCore.Relational
// 3.EFCore项目安装：Microsoft.EntityFrameworkCore.Sqlite
// 4.ASP.NET Core项目引用EFCore项目，并且通过Services.AddDbContext
//   来注入DbContext以及对DbContext进行配置
// 5.Controller中就可以注入DbContext类使用
// 6.让开发环境的Migration知道连接哪个数据库，在EFCore项目中创建
//   一个实现了IDesignTimeDbContextFactory的类
//   并且在CreateDbContext返回一个连接开发数据库的DbContext
//   * 看EFCoreBooks.MyDbContextDesignFactory
// 7.EFCore项目安装：Microsoft.EntityFrameworkCore.Tools
// 8.把启动项目设置为EFCore项目，进行Migration

var builder = WebApplication.CreateBuilder(args);

// 通过Services.AddDbContext来注入DbContext以及对DbContext进行配置
// AddDbContext的生命周期是Scoped，用AddDbContextPool会带来性能提升，但不推荐
builder.Services.AddDbContext<MyDbContext>(opt =>
{
    var folder = Environment.SpecialFolder.DesktopDirectory;
    string path = Environment.GetFolderPath(folder);
    string DbPath = Path.Join(path, "TestDB.db");
    opt.UseSqlite($"Data Source={DbPath}");
});

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
