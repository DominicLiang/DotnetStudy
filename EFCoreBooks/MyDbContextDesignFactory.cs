using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreBooks
{
    // 这个类是给Migration用的，所以只会在开发环境使用
    // 生产环境是不会用到的，所以不用太正规，但是要保证上传Github的安全
    internal class MyDbContextDesignFactory : IDesignTimeDbContextFactory<MyDbContext>
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
}
