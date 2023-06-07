using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PermissionArea : BaseModel
{
    public string Name { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}

public class PermissionAreaConfig : IEntityTypeConfiguration<PermissionArea>
{
    public void Configure(EntityTypeBuilder<PermissionArea> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(300);

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}