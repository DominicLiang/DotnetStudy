using JWTLogin.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTLogin.Data;

public class DataContext : IdentityDbContext<User, Role, Guid>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //从当前程序集加载所有的IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
