using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class BaseModel
{
    public long Id { get; set; }
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}

//public class BaseModelConfig : IEntityTypeConfiguration<BaseModel>
//{
//    public void Configure(EntityTypeBuilder<BaseModel> builder)
//    {
//        builder.HasKey(b => b.Id);
//    }
//}