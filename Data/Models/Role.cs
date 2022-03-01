using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;
public class Role : BaseModel
{
    public string RoleTitle { get; set; }

    public string RoleDescription { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.RoleTitle)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.RoleDescription)
            .HasMaxLength(250);
    }
}