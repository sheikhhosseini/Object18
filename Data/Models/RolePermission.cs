using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;
public class RolePermission : BaseModel
{
    public long PermissionId { get; set; }

    public long RoleId { get; set; }

    public Permission Permission { get; set; }

    public Role Role { get; set; }
}

public class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasOne(ur => ur.Permission)
            .WithMany(u => u.RolePermissions)
            .HasForeignKey(ur => ur.PermissionId);

        builder.HasOne(ur => ur.Role)
            .WithMany(u => u.RolePermissions)
            .HasForeignKey(ur => ur.RoleId);
    }
}