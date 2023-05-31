using Microsoft.EntityFrameworkCore;

namespace _08_ASP.NET筛选器
{
    public class MyDbContext:DbContext
    {
        public DbSet<Book> books { get; set; }
        public DbSet<Person> persons { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> opt):base(opt)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
