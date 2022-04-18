using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class LogCenter
{
    public long Id { get; set; }
    public string EntityName { get; set; }
    public string EntityOldValues{ get; set; }
    public string EntityNewValues { get; set; }
    public LogType Action { get; set; }
    public DateTime CreateDate { get; set; }
}

public enum LogType
{
    Create = 10,
    Update = 15,
    Delete = 20,
    SoftDelete = 25
}

public class LogCenterConfiguration : IEntityTypeConfiguration<LogCenter>
{
    public void Configure(EntityTypeBuilder<LogCenter> builder)
    {
        builder.HasKey(lc => lc.Id);

        builder.Property(lc => lc.EntityName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(lc => lc.Action)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(lc => lc.CreateDate)
            .IsRequired();
    }
}