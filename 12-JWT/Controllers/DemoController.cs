using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _12_JWT.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DemoController : ControllerBase
{
    // JWT（Json Web Token）
    // header + payload + seckey
    // 包：System.IdentityModel.Tokens.Jwt
    // 不要把不能被客户端知道的信息放到JWT中
    // 步骤
    // 1.包：Microsoft.AspNetCore.Authentication.JwtBearer
    // 2.配置JWT节点，节点下创建SigningKey、ExpireSeconds两个配置项
    //   分别代表JWT的密钥和过期时间
    // 3.创建配置类JWTOptions，包含上面两个属性

    private readonly static string secKey = Guid.NewGuid().ToString();

    [HttpGet]
    public string GetJWT()
    {
        return JWTCreater("yjk", new string[] { "admin" }, secKey);
    }

    [HttpGet]
    public string GetFakeSecKeyJWT()
    {
        return JWTCreater("yjk", new string[] { "admin" }, Guid.NewGuid().ToString());
    }

    [HttpPost]
    public void ShowJWT(string jwt)
    {
        JWTReader(jwt, secKey);
    }

    private string JWTCreater(string userName, string[] roles, string key)
    {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, userName));
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        DateTime expires = DateTime.Now.AddDays(1);

        byte[] secBytes = Encoding.UTF8.GetBytes(key);
        var secKey = new SymmetricSecurityKey(secBytes);
        var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
        string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return jwt;
    }

    private void JWTReader(string jwt, string seckey)
    {
        JwtSecurityTokenHandler handler = new();
        TokenValidationParameters para = new();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(seckey));
        para.IssuerSigningKey = securityKey;
        para.ValidateIssuer = false;
        para.ValidateAudience = false;
        ClaimsPrincipal claimsPrincipal = handler.ValidateToken(jwt, para, out SecurityToken secToken);
        foreach (var claim in claimsPrincipal.Claims)
        {
            Console.WriteLine($"{claim.Type} = {claim.Value}");
        }
    }
}


