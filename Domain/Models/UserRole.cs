using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Models;

public class UserRole : BaseModel
{
    public long User_Id { get; set; }

    public long Role_Id { get; set; }

    public User User { get; set; }

    public Role Role { get; set; }
}

public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.User_Id);

        builder.HasOne(ur => ur.Role)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.Role_Id);
    }
}