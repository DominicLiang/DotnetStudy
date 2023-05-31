var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddMemoryCache();

// 注册redis服务
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = "localhost";
    opt.InstanceName = "cache";
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 服务器端响应缓存 必须在MapControllers之前 不推荐用
app.UseResponseCaching();
app.MapControllers();

app.Run();
