using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;
public class Permission : IHaveId
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long PermissionAreaId { get; set; }

    public PermissionArea PermissionArea { get; set; }

    public long PermissionModuleId { get; set; }

    public PermissionModule PermissionModule { get; set; }

    public long PermissionActionId { get; set; }

    public PermissionAction PermissionAction { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; }
}

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(250);

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique();

        // Relations
        builder.HasOne(d => d.PermissionArea)
            .WithMany(p => p.Permissions)
            .HasForeignKey(d => d.PermissionAreaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.PermissionModule)
            .WithMany(p => p.Permissions)
            .HasForeignKey(d => d.PermissionModuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.PermissionAction)
            .WithMany(p => p.Permissions)
            .HasForeignKey(d => d.PermissionActionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}