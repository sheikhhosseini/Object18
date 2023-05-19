using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Member : BaseModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MobileNumber { get; set; }

    public string Email { get; set; }

    public bool Gender { get; set; }

    public string KodMeli { get; set; }

    public string DateOfBirth { get; set; }
        
    public string RegisterDate { get; set; }

    public string Image { get; set; }

    public string Address { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}

public class MemberConfig : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.MobileNumber)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(u => u.KodMeli)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(u => u.DateOfBirth)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(u => u.RegisterDate)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(50);

        builder.Property(u => u.Address)
            .HasMaxLength(400);

        builder.Property(u => u.Image)
            .HasMaxLength(200);

        // index
        builder.HasIndex(u=>u.MobileNumber)
            .IsUnique();

        builder.HasIndex(u => u.KodMeli)
            .IsUnique();
    }
}