using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;
public class Permission : BaseModel
{
    public string PermissionName { get; set; }

    public string PermissionLabel { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; }
}

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(r => r.PermissionName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(r => r.PermissionLabel)
            .IsRequired()
            .HasMaxLength(300);
    }
}