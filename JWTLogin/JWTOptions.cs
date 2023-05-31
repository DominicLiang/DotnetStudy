namespace JWTLogin;

public class JWTOptions
{
    //代表这个JWT的签发主体
    public string Issuer { get; set; } = string.Empty;
    //代表这个JWT的接收对象
    public string Audience { get; set; } = string.Empty;
    public string SecKey { get; set; } = string.Empty;
    public int Expire { get; set; }
}
