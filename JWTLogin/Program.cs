using JWTLogin;
using JWTLogin.Data;
using JWTLogin.Filters;
using JWTLogin.JWTService;
using JWTLogin.Middleware;
using JWTLogin.Model;
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
                            ValidateAudience = false,
                            ValidAudience = "audience",

                            // 验证过期时间和生命周期
                            RequireExpirationTime = true,
                            ValidateLifetime = true,

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

//app.UseJWTRefresh();

app.MapControllers();

app.Run();
