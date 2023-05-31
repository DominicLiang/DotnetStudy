using DotnetSpider;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DotnetSpider.Http;
using DotnetSpider.Infrastructure;
using DotnetSpider.Scheduler.Component;
using DotnetSpider.Scheduler;
using Microsoft.Extensions.Hosting;
using Serilog;
using DotnetSpider.Downloader;
using DotnetSpider.Selector;
using System.Text;

namespace _15_DotnetSpider;

public class StudySpider : Spider
{
    public static async Task RunAsync()
    {

        var builder = Builder.CreateDefaultBuilder<StudySpider>(options =>
        {
            // 每秒 1 个请求
            options.Speed = 1;
        });
        builder.UseSerilog();
        builder.UseDownloader<HttpClientDownloader>();
        builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
        await builder.Build().RunAsync();
    }

    public StudySpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
    {
    }

    protected override async Task InitializeAsync(CancellationToken stoppingToken)
    {
        AddDataFlow(new NewsParser());
        AddDataFlow(new FileStorage());

        var request = new Request("https://movie.douban.com/chart");
        request.Timeout = 10000;
        request.Headers.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.41";

        await AddRequestsAsync(request);
    }

    protected override SpiderId GenerateSpiderId()
    {
        return new(ObjectId.CreateId().ToString(), "Douban");
    }

    protected class NewsParser : DataParser
    {
        public NewsParser()
        {
            SelectableBuilder = context =>
            {
                var text = Encoding.UTF8.GetString(context.Response.Content.Bytes);
                var uri = context.Request.RequestUri;
                var domain = uri.Port == 80 || uri.Port == 443
                    ? $"{uri.Scheme}://{uri.Host}"
                    : $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                return new HtmlSelectable(text, domain, context.Options.RemoveOutboundLinks);
            };
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            var movieList = context.Selectable.SelectList(Selectors.XPath("//tr[@class='item']"));
            foreach (var item in movieList)
            {
                var img = item.XPath("//img/@src")?.Value;
                var title = item.XPath("//div[@class='pl2']//a")?.Value.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", "");
                var actors = item.XPath("//p[@class='pl']")?.Value;
                var rate = item.XPath("//span[@class='rating_nums']")?.Value;
                context.AddData("img", img);
                context.AddData("title", title);
                context.AddData("actors", actors);
                context.AddData("rate", rate);
            }

            return Task.CompletedTask;
        }
    }

    public class Moives
    {

        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Actors { get; set; }
        public string Rate { get; set; }
    }
}

