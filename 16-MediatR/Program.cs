using _16_MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

{
    //°²×°°ü: MediatR
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        //cfg.AddBehavior<IPipelineBehavior<Ping, Pong>, PingPongBehavior>();
        //cfg.AddOpenBehavior(typeof(GenericBehavior<,>));
    });
}

builder.Services.AddDbContext<MyDbContext>(opt =>
{
    var folder = Environment.SpecialFolder.DesktopDirectory;
    string path = Environment.GetFolderPath(folder);
    string DbPath = Path.Join(path, "TestDB.db");
    opt.UseSqlite($"Data Source={DbPath}");
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

app.MapControllers();

app.Run();
