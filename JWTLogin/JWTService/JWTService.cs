using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTLogin.JWTService;

public class JWTService : IJWTService
{
    public string CreateJWT(IEnumerable<Claim> claims, JWTOptions options)
    {
        DateTime expires = DateTime.Now.AddMinutes(options.Expire);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims, DateTime.Now, expires, credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
