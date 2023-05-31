// ASP.NET Core会从环境变量中读取名字为ASPNETCORE_ENVIRONMENT的值
// 推荐值：Development(开发环境)、Staging(测试环境)、Production(生产环境)

// 更改环境变量
// 项目右键属性-调试-打开调试启动配置文件UI

var builder = WebApplication.CreateBuilder(args);

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



// 配置文件的机密泄漏
// 如果把重要账号密码或者数据库连接代码直接写在配置文件中
// 在上传Github的是时候就会泄漏这些信息
// 需要写一个不存在于项目中，但是能读取的配置文件，这样来防止泄漏
// VS在项目中右键-管理用户机密就能创建这样的配置文件
// 这个配置文件虽然不存在项目中，但是项目能读取 如下
if (app.Environment.IsDevelopment())
{
    string s = builder.Configuration.GetSection("connStr").Value;
    Console.WriteLine(s);
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
