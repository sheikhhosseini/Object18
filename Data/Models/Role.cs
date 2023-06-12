using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;
public class Role : IHaveId, IHaveDateLog, IHaveConcurrencyStamp
{
    public long Id { get; set; }

    public string CreateDate { get; set; }

    public string LastUpdateDate { get; set; }

    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public string Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; }
}

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(250);

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}