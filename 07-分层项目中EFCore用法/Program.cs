using EFCoreBooks;
using Microsoft.EntityFrameworkCore;
// �ٷ��� װ�������
// Microsoft.EntityFrameworkCore.Relational

// EFCore����
// 1.�������Ŀ����ʵ���ࡢDbContext���������
//   DbContext�в��������ݿ����ӣ�����ΪDbContextдһ�����캯��
//   �����DbContextOptions���͵Ĳ����磺
//   (DbContextOptions<MyDbContext> options) : base(options)
// 2.EFCore��Ŀ��װ��Microsoft.EntityFrameworkCore.Relational
// 3.EFCore��Ŀ��װ��Microsoft.EntityFrameworkCore.Sqlite
// 4.ASP.NET Core��Ŀ����EFCore��Ŀ������ͨ��Services.AddDbContext
//   ��ע��DbContext�Լ���DbContext��������
// 5.Controller�оͿ���ע��DbContext��ʹ��
// 6.�ÿ���������Migration֪�������ĸ����ݿ⣬��EFCore��Ŀ�д���
//   һ��ʵ����IDesignTimeDbContextFactory����
//   ������CreateDbContext����һ�����ӿ������ݿ��DbContext
//   * ��EFCoreBooks.MyDbContextDesignFactory
// 7.EFCore��Ŀ��װ��Microsoft.EntityFrameworkCore.Tools
// 8.��������Ŀ����ΪEFCore��Ŀ������Migration

var builder = WebApplication.CreateBuilder(args);

// ͨ��Services.AddDbContext��ע��DbContext�Լ���DbContext��������
// AddDbContext������������Scoped����AddDbContextPool��������������������Ƽ�
builder.Services.AddDbContext<MyDbContext>(opt =>
{
    var folder = Environment.SpecialFolder.DesktopDirectory;
    string path = Environment.GetFolderPath(folder);
    string DbPath = Path.Join(path, "TestDB.db");
    opt.UseSqlite($"Data Source={DbPath}");
});

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
