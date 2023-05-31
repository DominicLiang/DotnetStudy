using System.Security.Claims;

namespace JWTLogin.JWTService;

public interface IJWTService
{
    public string CreateJWT(IEnumerable<Claim> claims, JWTOptions options);
}
