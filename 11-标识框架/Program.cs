using _11_��ʶ���;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


// Identity��ܵ�ʹ��
// 1.IdentityUser<TKey>��IdentityRole<TKey>,TKey��������������
//   ����һ���д�̳�������������Զ����࣬���������Զ�������
// 2.��װ����Microsoft.AspNetCore.Identity.EntityFrameworkCore
// 3.�����̳���IdentityDbContext����
// 4.����ͨ��IdDbContext�����������ݿ�
//   ����������ṩRoleManager��UserManager�������򻯶����ݿ�Ĳ���
// 5.���ַ����ķ���ֵΪTask<IdentityResult>���ͣ��鿴�����ⶨ��
// 6.������ע��������ע���ʶ�����ط���
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

{
    // 6.������ע��������ע���ʶ�����ط���
builder.Services.AddDbContext<MyDbContext>(opt =>
{
var folder = Environment.SpecialFolder.DesktopDirectory;
string path = Environment.GetFolderPath(folder);
string DbPath = Path.Join(path, "TestDB.db");
opt.UseSqlite($"Data Source={DbPath}");
});

{
    // 6.������ע��������ע���ʶ�����ط���
    builder.Services.AddDataProtection();
    builder.Services.AddIdentityCore<MyUser>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
        options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    });
    IdentityBuilder idBuilder = new IdentityBuilder(typeof(MyUser), typeof(MyRole), builder.Services);
    idBuilder.AddEntityFrameworkStores<MyDbContext>()
             .AddDefaultTokenProviders()
             .AddUserManager<UserManager<MyUser>>()
             .AddRoleManager<RoleManager<MyRole>>();
}


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
