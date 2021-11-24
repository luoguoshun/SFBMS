using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFBMS.Entity.SystemModule;

namespace SFBMS.Entity.EntityConfiguration
{
    public class UserRoleTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(k => new { k.RoleId, k.UserId });

            builder.HasOne(c => c.Client)
                   .WithMany(r => r.Roles)
                   .HasForeignKey(k => k.UserId);

            builder.HasOne(r => r.Role)
                   .WithMany(r => r.Roles)
                   .HasForeignKey(k => k.RoleId);

        }
    }
}
