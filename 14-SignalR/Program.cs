
// SignalR
// .NET Core下对WebSocket的封装



using _14_SignalR;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

{
    builder.Services.AddSignalR();

    string[] urls = new[] { "http://localhost:5173" };
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(b => b.WithOrigins(urls)
                                       .AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowCredentials());
    });
}





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

{
    app.MapHub<MyHub>("/MyHub");

    app.UseCors();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
