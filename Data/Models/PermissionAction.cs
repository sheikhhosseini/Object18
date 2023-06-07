using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PermissionAction : BaseModel
{
    public string Name { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}

public class PermissionActionConfig : IEntityTypeConfiguration<PermissionAction>
{
    public void Configure(EntityTypeBuilder<PermissionAction> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(300);

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}