var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����Cors ʹ����˽�������ͻ��˵�����
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(b =>
    {
        b.WithOrigins(new string[] 
        {
            // ��� ���Ӻ��治�ܼ�/б��
            "http://localhost:5173"
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});//

var app = builder.Build();

{
    // ʹ����˽�������ͻ��˵�����
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
