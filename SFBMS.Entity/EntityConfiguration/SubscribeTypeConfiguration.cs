using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFBMS.Entity.BookModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Entity.EntityConfiguration
{
    public class SubscribeTypeConfiguration : IEntityTypeConfiguration<Subscribe>
    {
        public void Configure(EntityTypeBuilder<Subscribe> builder)
        {
            builder.HasKey(k => new { k.BookId, k.ClientNo });
        }
    }
}
