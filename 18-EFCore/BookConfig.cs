using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore;

// 可以不建 约定大于配置
class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("T_Books");
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.AuthorName).HasMaxLength(20);

        builder.Ignore(b => b.director);// 不映射某个属性
        builder.Property(x => x.Description).HasColumnName("D"); // 改变映射列名
        builder.Property(x => x.test).HasColumnType("varchar"); // 标明数据库类型
        //builder.HasKey(x => x.Price); // 标明主键
        builder.Property(x => x.Description).HasDefaultValue("HelloWrold");// 默认值
        builder.HasIndex(x => x.Title).IsUnique(); // 索引/唯一索引
        builder.HasIndex(x => new { x.Title, x.AuthorName});// 复合索引


    }
}