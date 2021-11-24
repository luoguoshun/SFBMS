using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFBMS.Entity.ClientModule;
using SFBMS.Entity.SystemModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Entity.EntityConfiguration
{
    public class ClientTypeConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
        }
    }
}
