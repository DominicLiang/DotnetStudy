using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddNacosAspNet(builder.Configuration);

builder.Services.AddNacosAspNet(x =>
{
    x.ServerAddresses = new List<string> { "http://localhost:8848/" };
    x.EndPoint = "";
    x.Namespace = "cs";
    x.ServiceName = "App2";
    x.GroupName = "DEFAULT_GROUP";
    x.ClusterName = "DEFAULT";
    x.Ip = "";
    x.PreferredNetworks = "";
    x.Port = 0;
    x.Weight = 100;
    x.RegisterEnabled = true;
    x.InstanceEnabled = true;
    x.Ephemeral = true;
    x.Secure = false;

    // swich to use http or rpc
    x.NamingUseRpc = true;
});

//Console.WriteLine(builder.Configuration.GetSection("Test").Value);

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
