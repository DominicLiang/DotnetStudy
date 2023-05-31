using System.Text;
using TencentCloud.Common;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

try
{
    // 为了保护密钥安全，建议将密钥设置在环境变量中或者配置文件中。
    // 硬编码密钥到代码中有可能随代码泄露而暴露，有安全隐患，并不推荐。
    // 这里采用的是从环境变量读取的方式，需要在环境变量中先设置这两个值。
    Credential cred = new Credential
    {
        SecretId = Environment.GetEnvironmentVariable("AKIDRi0FXimiYI6vdEsX8wb7eaQRI3f8Shz7"),
        SecretKey = Environment.GetEnvironmentVariable("zuoiTAhz36AB1ksq7B1cz84qPk192GQW")
    };
    SmsClient client = new SmsClient(cred, "ap-guangzhou");
    SendSmsRequest request = new SendSmsRequest()
    {
        PhoneNumberSet = new string[] { "+86" + "17520322982" },
        SmsSdkAppid = "1400832128",
        Sign = "Unity特效学习公众号",
        TemplateID = "1839066",
        TemplateParamSet = new string[] { GenerateRandomCode(6) },
    };
    SendSmsResponse response = await client.SendSms(request);
    Console.WriteLine(AbstractModel.ToJsonString(response));
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}
/// <summary>
/// 用GUID作种子生成理论上不重复的随机数作为验证码
/// </summary>
/// <param name="length"></param>
/// <returns></returns>
static string GenerateRandomCode(int length)
{
    var result = new StringBuilder();
    for (var i = 0; i < length; i++)
    {
        var r = new Random(Guid.NewGuid().GetHashCode());
        result.Append(r.Next(0, 10).ToString());
    }
    string code = result.ToString();

    return code;
}