using Microsoft.AspNetCore.Identity;

namespace JWTLogin.Model;

public class User:IdentityUser<Guid>
{
    [ProtectedPersonalData]
    public string Token { get; set; }
}
