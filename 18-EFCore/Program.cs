using Microsoft.EntityFrameworkCore;

namespace _18_EFCore;

// 什么是ORM
// ORM：Object Relational Mapping
// 让开发者用对象操作的形式来操作关系型数据库
// EF Core 微软官方提供ORM

// 用什么数据库
// 1.EFCore是对于底层ADO.NET Core的封装
// 2.EFCore支持所有主流数据库
// 3.对于SQLServer支持最完美，MySQL

// 安装包
// Microsoft.EntityFrameworkCore.Sqlite 后面带数据库名表示支持哪个数据库
// Microsoft.EntityFrameworkCore.Tools  对数据迁移回滚的支持

// 使用步骤
// *创建配置类（可选）配置类继承IEntityTypeConfiguration泛型为数据类类型
// 如果使用配置类需要在context类中重写OnModelCreating加入下面内容
// modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
// 1.创建数据类
// 2.创建一个继承DbContext的context类
// 3.在context类中创建DbSet属性，泛型类型为数据类
// 4.context类中重写OnConfiguring，并optionsBuilder.UseSqlite写入数据库连接代码
// 5.在控制台中使用Add-Migration xxx来生成迁移代码
// 6.在控制台中使用Update-database来执行迁移

// 约定配置
// 主要规则：
// 1.表名采用DbContext的对应的DbSet的属性名
// 2.数据表列的名字采用实体类属性的名字
//   列的数据的类型采用和实体类属性类型最兼容的类型
// 3.数据表列的可空性取决于对应实体类型属性的可空性
// 4.名字为Id的属性为主键，如果主键为shrot，int或者long类型，则默认采取自增字段
//   如果主键为Guid类型，则默认采用默认的Guid生成机制生成主键值

// 两种配置方式
// 1.DataAnnotation   把配置以特性的形式标注在实体类中  看Dog.cs
// 2.FluentAPI        把配置写在单独的配置类中         看Books.cs根BookConfig.cs

// 主键
// 1.自增
// 2.GUID 不连续 GUID做主键的时候不能把主键作为聚集索引，因为聚集索引使按照顺序保存主键的
// 3.混合自增和GUID
// 4.Hi/Lo

// 迁移命令             xxx为迁移名称
// Update-Database                   更新数据库到最新迁移
// Update-Database xxx               更新数据库到给定迁移
// Add-Migration xxx                 增加新的迁移
// Remove-Migration                  删除上一个迁移
// Get-Migration                     获取迁移列表
// Script-Migration                  生成一个从空白数据库到最新迁移的SQL
// Script-Migration xxx              生成一个从给定迁移到最新迁移的SQL
// Script-Migration fromxxx toxxx    生成一个从指定迁移到指定迁移的SQL

// 其他命令
// Scaffold-DbContext 数据库连接 EFCore对应数据库包名     根据已有的数据库生成类

internal class Program
{
    static void Main(string[] args)
    {
        // Context=逻辑上的数据库
        using (TestDbContext ctx = new TestDbContext())
        {
            {
                //// 插入数据

                //ctx.Books.Add(new Book()
                //{
                //    AuthorName = "3",
                //    Price = 2,
                //    PubTime = DateTime.Now,
                //    Title = "qqq"
                //});

                //ctx.SaveChangesAsync();
            }
            {
                //// 使用LINQ来查询
                //var result = from b in ctx.Books
                //             where b.Price > 5
                //             select b;
                //foreach (var item in result)
                //{
                //    Console.WriteLine(item.Title);
                //}

                //var book = ctx.Books.Single(b => b.Title == "水杯");
                //Console.WriteLine(book.AuthorName);

                //var result2=from b in ctx.Books
                //            orderby b.Price descending
                //            select b;
                //foreach (var item in result2)
                //{
                //    Console.WriteLine(item.Title+"  "+item.Price);
                //}

                //var cat = ctx.Cats.Single(c => c.ID == 1);
                //Console.WriteLine(cat.ID + "  " + cat.FullName + "  " + cat.FirstMidName + "  " + cat.LastName + "  " + cat.EnrollmentDate);
            }
            {
                //// 修改数据
                //var b=ctx.Books.Single(b=>b.Title=="qqq");
                //b.Title = "电风扇";
                //ctx.SaveChangesAsync();
            }
            {
                //// 删除数据
                //var b = ctx.Books.Single(b=>b.Title=="HelloWorld");
                //ctx.Books.Remove(b);
                //ctx.SaveChangesAsync();
            }
            {
                //// 批量修改 注意批量操作不用SaveChanges
                //(from b in ctx.Books
                // where b.Price < 10
                // select b).ExecuteUpdateAsync(s => s
                // .SetProperty(b => b.Price, b => b.Price + 1)
                // .SetProperty(b => b.Title, b => b.Title + " UP"));
            }
            {
                //// 批量删除 注意批量操作不用SaveChanges
                //(from b in ctx.Books
                // where b.Price < 8
                // select b).ExecuteDeleteAsync();
            }
        }
    }
}
