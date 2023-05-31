using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace _11_标识框架;

public class MyDbContextDesignFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<MyDbContext> builder = new DbContextOptionsBuilder<MyDbContext>();

        var folder = Environment.SpecialFolder.DesktopDirectory;
        string path = Environment.GetFolderPath(folder);
        string DbPath = Path.Join(path, "TestDB.db");
        builder.UseSqlite($"Data Source={DbPath}");

        MyDbContext ctx = new MyDbContext(builder.Options);
        return ctx;
    }
}
