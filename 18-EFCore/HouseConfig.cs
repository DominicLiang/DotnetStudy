using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore
{
    public class HouseConfig : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder.ToTable("T_Houses");
            builder.Property(b=>b.Name).IsRequired();
            builder.Property(b=>b.Owner).IsConcurrencyToken();
            builder.Property(b=>b.Timestamp).IsRowVersion();
        }
    }
}
