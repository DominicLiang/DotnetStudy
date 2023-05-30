using Microsoft.Extensions.Logging;

namespace SystemServices
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }

    class Test2
    {
        private readonly ILogger<Test2> logger;
        public Test2(ILogger<Test2> logger)
        {
            this.logger = logger;
        }

        public void Test()
        {
            logger.LogDebug("开始执行FTP同步");
            logger.LogDebug("连接FTP成功");
            logger.LogWarning("查找数据失败，重试第一次");
            logger.LogWarning("查找数据失败，重试第二次");
            logger.LogError("查找数据最终失败");

            User user = new User(1, "admin", "123@123.com");
            logger.LogDebug("注册一个用户 {@user}", user);
        }
    }
}
