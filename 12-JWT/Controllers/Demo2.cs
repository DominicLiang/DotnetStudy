using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _12_JWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Demo2 : ControllerBase
    {
        private readonly IOptionsSnapshot<JWTOptions> jwtSetting;

        public Demo2(IOptionsSnapshot<JWTOptions> jwtSetting)
        {
            this.jwtSetting = jwtSetting;
            Console.WriteLine(jwtSetting.Value.SecKey);
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if (userName == "yzk" || password == "123")
            {
                string jwt = JWTCreater("yjk", new string[] { "admin" }, jwtSetting.Value.SecKey);
                return Ok(jwt);
            }
            else
            {
                return BadRequest();
            }
        }
        private string JWTCreater(string userName, string[] roles, string key)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName)
            };
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
    }
}
