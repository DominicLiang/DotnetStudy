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
// Microsoft.EntityFrameworkCore.Tools  数据迁移回滚的控制台命令

// 现在EFCore7.0.5可以支持批量修改和批量删除，但是还没有批量添加

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

// 代码查看EFCore生成的SQL语句
// 看Context类

// 实体间关系
// 看OneToOne.cs OneToMany.cs ManyToMany.cs

// 执行原生SQL语句
// 1.执行非查询原生SQL语句
// dbCtx.Database.ExecuteSqlInterpolatedAsync($"...")
// 2.实体相关SQL
// dbCtx.TableName.FromSqlInterpolated($"...")
// 3.执行任意原生SQL语句
// ctx.Database.ExecuteSqlRawAsync("");

// 快照更改跟踪
// 只要一个实体对象和DbContext发生任何关系都默认会被DbContext跟踪
// DbContext存了关系对象的一个备份，在执行SaveChanges的时候将对象和备份比对
// 以此识别对象的更改
// EntityEntry entry=DbCtx.Entry()
// EntityEntry为DbContext的跟踪类
// EntityEntry的State属性代表实体对象的状态
// 通过DebugView.LongView属性可以看到实体的变化信息

// EFCore优化AsNoTracking
// 只在需要修改的时候才需要跟踪
// 如果只是查看数据的话，跟踪会耗费内存
// 所以只查看的时候最好加上AsNoTracking()
// ctx.Books.AsNoTracking().Where(....)

// 软删除
// 设置一个IsDeleted列 如果值为true就认为是删除，实际没有真正的删除

// 全局查询筛选器
// 看BookConfig.cs
// IgnoreQueryFilters 忽略全局筛选器
// ctx.books.IgnoreQueryFilters().Where(...)

// 并发控制
// 悲观策略：
// 加锁  EFCore没有悲观并发控制的功能
// 乐观策略：
// 并发令牌 * 并发令牌推荐用GUID做，每次更新数据时new Guid（）
// 看LikeConfig.cs
// 出现并发时会抛DbUpdateConcurrencyException异常

public class Program
{
    static async Task Main(string[] args)
    {
        // Context=逻辑上的数据库
        // 最好一个请求一个context
        // 一个service一个context
        // 多个请求/多线程 要用不同的context
        using (TestDbContext ctx = new TestDbContext())
        {
            {
                // 省掉Select语句 不推荐
                // 强行将属性标识为已修改状态
                //Book b = new Book() { Id = 1, Price = 2 };
                //ctx.Entry(b).Property(b => b.Price).IsModified = true;
                //ctx.SaveChanges();
            }
            {
                // 省掉Select语句 不推荐
                // 强行将id为1的对象标识为删除
                //Book b = new Book() { Id = 1 };
                //ctx.Entry(b).State = EntityState.Deleted;
                //ctx.SaveChanges();
            }
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

                //var result2 = from b in ctx.Books
                //              orderby b.Price descending
                //              select new
                //              {
                //                  Title = b.Title,
                //                  Price = b.Price,
                //              };
                //foreach (var item in result2)
                //{
                //    Console.WriteLine(item.Title + "  " + item.Price);
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

        // 并发控制
        TestDbContext ctx1 = new TestDbContext();
        TestDbContext ctx2 = new TestDbContext();

        var l1 = await ctx1.likes.SingleAsync(l => l.Id == 1);
        var l2 = await ctx2.likes.SingleAsync(l => l.Id == 1);

        try
        {
            l1.LikeNumber++;
            await ctx1.SaveChangesAsync();

            l2.LikeNumber++;
            await ctx2.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is Like)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        // TODO: decide which value should be written to database
                        // proposedValues[property] = <value to be saved>;
                    }

                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }
        }
    }
}
