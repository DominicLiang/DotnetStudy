// JWT（Json Web Token）
// header + payload + seckey


using _12_JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
    {
        var JwtOptions = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
        byte[] keyBytes = Encoding.UTF8.GetBytes(JwtOptions.SecKey);
        var secKey = new SymmetricSecurityKey(keyBytes);
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = secKey
        };
    });
}



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
