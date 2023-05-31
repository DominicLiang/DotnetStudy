using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _11_标识框架;

public class MyDbContext : IdentityDbContext<MyUser, MyRole, long>
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

    }
}
