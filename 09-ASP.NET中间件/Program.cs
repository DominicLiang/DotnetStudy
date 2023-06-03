using _09_ASP.NET�м��;

// �м������������
// Map��Use��Run
// Map��������һ���ܵ����Դ�����Щ����
// Use��Run��������ܵ�
// һ���ܵ������ɸ�Use��һ��Run���
// ÿ��Use����һ���м��
// ��Run������ִ�����յĺ���Ӧ���߼�

// Filter��Middleware������
// �м����ASP.NET Core��������ṩ�Ĺ��ܣ���Filter��ASP.NET Core MVC���ṩ�Ĺ���
// ASP.NET Core MVC����MVC�м���ṩ�Ŀ�ܣ���Filter����MVC�м���ṩ�Ĺ���

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// �����м��
app.Map("/test", async (pipeBuilder) =>
{
    pipeBuilder.UseMiddleware<CheckMiddleware>();
    pipeBuilder.Run(async context =>
    {
        dynamic? obj = context.Items["BodyJson"];
        if (obj != null)
        {
            await context.Response.WriteAsync($"{obj}+<br/>");
        }
    });
});

app.Run();
