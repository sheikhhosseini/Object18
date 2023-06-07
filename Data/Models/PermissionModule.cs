using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PermissionModule : BaseModel
{
    public string Name { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}

public class PermissionModuleConfig : IEntityTypeConfiguration<PermissionModule>
{
    public void Configure(EntityTypeBuilder<PermissionModule> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(300);

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}