using JWTLogin;
using JWTLogin.Data;
using JWTLogin.Filters;
using JWTLogin.JWTService;
using JWTLogin.Model;
using JWTLogin.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MvcOptions>(opt =>
{
    opt.Filters.Add<JWTValidationFilter>();
    opt.Filters.Add<JWTRefreshFilter>();
});

builder.Services.AddScoped<IJWTService, JWTService>();

{
    //解决分布式问题,需要Redis
    //包：Microsoft.AspNetCore.SignalR.StackExchangeRedis
    builder.Services.AddSignalR().AddStackExchangeRedis("localhost", opt =>
    {
        opt.Configuration.ChannelPrefix = "SignalR_";
    });
}
{
    builder.Services.AddDbContext<DataContext>(opt =>
    {
        var folder = Environment.SpecialFolder.DesktopDirectory;
        string path = Environment.GetFolderPath(folder);
        string DbPath = Path.Join(path, "TestDB.db");
        opt.UseSqlite($"Data Source={DbPath}");
    });

    // 6.向依赖注入容器中注册标识框架相关服务
    builder.Services.AddDataProtection();
    builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
        options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    });
    IdentityBuilder idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
    idBuilder.AddEntityFrameworkStores<DataContext>()
             .AddDefaultTokenProviders()
             .AddUserManager<UserManager<User>>()
             .AddRoleManager<RoleManager<Role>>();
}

{
    // Swagger 加授权按钮
    builder.Services.AddSwaggerGen(c =>
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Authorization"
            },
            Scheme = "oauth2",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
        };
        c.AddSecurityDefinition("Authorization", scheme);
        var requirement = new OpenApiSecurityRequirement();
        requirement[scheme] = new List<string>();
        c.AddSecurityRequirement(requirement);
    });
}

{

    // JwtBearer设置
    builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
    var JwtOptions = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
    Console.WriteLine(JwtOptions.SecKey);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(JwtOptions.SecKey);
                        var secKey = new SymmetricSecurityKey(bytes);
                        opt.TokenValidationParameters = new()
                        {
                            // 验证密钥
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = secKey,

                            // 验证发行人
                            ValidateIssuer = true,
                            ValidIssuer = "issuer",

                            // 验证订阅人
                            ValidateAudience = true,
                            ValidAudience = "audience",

                            // 验证过期时间和生命周期
                            RequireExpirationTime = true,
                            ValidateLifetime = true,

                        };

                        // SignalR的JWT认证
                        // websocket不支持自定义报文头
                        // 所以jwt需要通过url中的querystring传递
                        // 然后在服务器的OnMessageReceived中
                        // 把querystring中的jwt读出来赋值给context.token
                        opt.Events = new JwtBearerEvents()
                        {
                            OnMessageReceived = (context) =>
                            {
                                var accessToken = context.Request.Query["access_token"];
                                var path = context.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken)
                                && path.StartsWithSegments("/MyHub"))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });
}

{
    // 跨域Cors 使服务端接受这个客户端的请求
    builder.Services.AddCors(opt =>
    {
        opt.AddDefaultPolicy(b =>
        {
            b.WithOrigins(new string[] { "http://localhost:5173" })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    });
}

var app = builder.Build();

{
    app.UseCors();
    app.MapHub<MyHub>("/MyHub");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
