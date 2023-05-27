namespace _14_爬虫;

// 1.爬虫，爬虫攻防
// 2.下载html      log4net(日志包)
// 3.xpath解析html，获取数据和深度抓取
// 4.惰性加载、Ajax加载数据、VUE数据绑定
// 5.不一样的属性和Ajax数据的获取
// 6.多线程抓取

// 爬虫：是一个自动提取网页的程序
//      URL开始 => 分析获取数据和抓到URL => 递归 => 结束
//      下载html => 解析获取数据 => 数据保存
// 爬虫攻防：robots协议 => 君子协定(robots.txt)
//         请求检测header => 爬虫模拟
//         用户登录 => 请求带上cookie
//         爬虫频率高限制IP(黑名单/返回验证码) => 多个IP(adsl拨号/168伪装IP/代理IP)
//         验证码 => 开源组件图片识别/打码平台
// 大招：数据动态加载
//      转图片
//      js收集用户操作
//      用户控件（可以收集更多信息）
internal class Program
{
    static void Main(string[] args)
    {
        
    }
}