namespace Data.Models;

public interface IHaveConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}