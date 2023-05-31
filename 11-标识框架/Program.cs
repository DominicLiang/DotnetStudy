using _11_标识框架;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


// Identity框架的使用
// 1.IdentityUser<TKey>、IdentityRole<TKey>,TKey代表主键的类型
//   我们一般编写继承自这两个类的自定义类，可以增加自定义属性
// 2.安装包：Microsoft.AspNetCore.Identity.EntityFrameworkCore
// 3.创建继承自IdentityDbContext的类
// 4.可以通过IdDbContext类来操作数据库
//   不过框架中提供RoleManager、UserManager等类来简化对数据库的操作
// 5.部分方法的返回值为Task<IdentityResult>类型，查看、讲解定义
// 6.向依赖注入容器中注册标识框架相关服务
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

{
    // 6.向依赖注入容器中注册标识框架相关服务
builder.Services.AddDbContext<MyDbContext>(opt =>
{
var folder = Environment.SpecialFolder.DesktopDirectory;
string path = Environment.GetFolderPath(folder);
string DbPath = Path.Join(path, "TestDB.db");
opt.UseSqlite($"Data Source={DbPath}");
});

{
    // 6.向依赖注入容器中注册标识框架相关服务
    builder.Services.AddDataProtection();
    builder.Services.AddIdentityCore<MyUser>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
        options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    });
    IdentityBuilder idBuilder = new IdentityBuilder(typeof(MyUser), typeof(MyRole), builder.Services);
    idBuilder.AddEntityFrameworkStores<MyDbContext>()
             .AddDefaultTokenProviders()
             .AddUserManager<UserManager<MyUser>>()
             .AddRoleManager<RoleManager<MyRole>>();
}


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
