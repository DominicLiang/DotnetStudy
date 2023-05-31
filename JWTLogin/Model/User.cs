using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace JWTLogin.Model;

public class User:IdentityUser<Guid>
{
    [ProtectedPersonalData]
    public string? Token { get; set; }
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id).HasMaxLength(128);
    }
}