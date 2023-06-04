using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;
public class Mission : BaseModel
{
    public string Title { get; set; }

    public string Description { get; set; }


    public Member Member { get; set; }

    public long MemberId { get; set; }
}

public class MissionConfig : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> builder)
    {
        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(250);

        // relations 
        builder.HasOne(ur => ur.Member)
            .WithMany(u => u.Missions)
            .HasForeignKey(ur => ur.MemberId);
    }
}