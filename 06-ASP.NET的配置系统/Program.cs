// ASP.NET Core��ӻ��������ж�ȡ����ΪASPNETCORE_ENVIRONMENT��ֵ
// �Ƽ�ֵ��Development(��������)��Staging(���Ի���)��Production(��������)

// ���Ļ�������
// ��Ŀ�Ҽ�����-����-�򿪵������������ļ�UI

using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
//string connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
//Console.WriteLine(connStr);
//builder.Host.ConfigureAppConfiguration((c, b) =>
//{
//    b.AddDbConfiguration(() =>
//    {
//        return new SqlConnection(connStr);
//    }, tableName: "config", reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(2));
//});
string s=builder.Configuration.GetSection("JWT").Value;
Console.WriteLine(s);

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



// �����ļ��Ļ���й©
// �������Ҫ�˺�����������ݿ����Ӵ���ֱ��д�������ļ���
// ���ϴ�Github����ʱ��ͻ�й©��Щ��Ϣ
// ��Ҫдһ������������Ŀ�У������ܶ�ȡ�������ļ�����������ֹй©
// VS����Ŀ���Ҽ�-�����û����ܾ��ܴ��������������ļ�
// ��������ļ���Ȼ��������Ŀ�У�������Ŀ�ܶ�ȡ ����
//if (app.Environment.IsDevelopment())
//{
//    string s = builder.Configuration.GetSection("connStr").Value;
//    Console.WriteLine(s);
//}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

class test
{
    public string a { get; set; }
    public string b { get; set; }
    public string c { get; set; }
}