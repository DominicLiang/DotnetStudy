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
    //����ֲ�ʽ����,��ҪRedis
    //����Microsoft.AspNetCore.SignalR.StackExchangeRedis
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

    // 6.������ע��������ע���ʶ�����ط���
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
    // Swagger ����Ȩ��ť
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

    // JwtBearer����
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
                            // ��֤��Կ
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = secKey,

                            // ��֤������
                            ValidateIssuer = true,
                            ValidIssuer = "issuer",

                            // ��֤������
                            ValidateAudience = true,
                            ValidAudience = "audience",

                            // ��֤����ʱ�����������
                            RequireExpirationTime = true,
                            ValidateLifetime = true,

                        };

                        // SignalR��JWT��֤
                        // websocket��֧���Զ��屨��ͷ
                        // ����jwt��Ҫͨ��url�е�querystring����
                        // Ȼ���ڷ�������OnMessageReceived��
                        // ��querystring�е�jwt��������ֵ��context.token
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
    // ����Cors ʹ����˽�������ͻ��˵�����
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
