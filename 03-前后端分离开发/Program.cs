var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 跨域Cors 使服务端接受这个客户端的请求
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(b =>
    {
        b.WithOrigins(new string[] 
        {
            // 大坑 链接后面不能加/斜杠
            "http://localhost:5173"
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});//

var app = builder.Build();

{
    // 使服务端接受这个客户端的请求
    app.UseCors();
}

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
