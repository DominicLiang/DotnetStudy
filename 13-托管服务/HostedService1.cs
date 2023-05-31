namespace _13_托管服务;

public class HostedService1 : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(3000);
        Console.WriteLine("HostedService启动");
        string s = await File.ReadAllTextAsync("D:\\Project\\Dotnet\\DotnetStudy\\13-托管服务\\TextFile.txt");
        await Task.Delay(3000);
        Console.WriteLine(s);
    }
}
