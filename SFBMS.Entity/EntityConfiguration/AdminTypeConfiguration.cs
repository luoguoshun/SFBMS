using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFBMS.Entity.SystemModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Entity.EntityConfiguration
{
    public class AdminTypeConfiguration : IEntityTypeConfiguration<AdminInfo>
    {
        public void Configure(EntityTypeBuilder<AdminInfo> builder)
        {
           
        }
    }
}
