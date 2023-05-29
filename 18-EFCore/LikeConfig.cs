using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore
{
    public class LikeConfig : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            // 设置并发令牌 这虽然用LikeNumber做 但是推荐用Guid做
            builder.Property(l => l.LikeNumber).IsConcurrencyToken();
        }
    }
}
