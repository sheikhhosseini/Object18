using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class User : IHaveId, IHaveDateLog, IHaveConcurrencyStamp
{
    public long Id { get; set; }

    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public string CreateDate { get; set; }

    public string LastUpdateDate { get; set; }

    public bool IsDelete { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MobileNumber { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ActiveCode { get; set; }

    public bool IsActive { get; set; }

    public string UserImage { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public ICollection<UserRole> UserRoles { get; set; }
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.MobileNumber)
            .HasMaxLength(11);

        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.ActiveCode)
            .HasMaxLength(100);

        builder.Property(u => u.UserImage)
            .HasMaxLength(200);

        builder.Property(e => e.ConcurrencyStamp)
            .IsConcurrencyToken();

        // index
        builder.HasIndex(u=>u.Email)
            .IsUnique();

        builder.HasIndex(u => u.MobileNumber)
            .IsUnique();
    }
}