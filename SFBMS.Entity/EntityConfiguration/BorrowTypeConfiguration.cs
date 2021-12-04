using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFBMS.Entity.OrderModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Entity.EntityConfiguration
{
    public class BorrowTypeConfiguration : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.HasOne(x => x.ToExamine)
                   .WithOne(x => x.Borrow)
                   .HasForeignKey<ToExamine>(k=>k.BorrowId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
