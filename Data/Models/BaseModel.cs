namespace Data.Models;

public interface IHaveId
{
    public long Id { get; set; }
}

public interface IHaveSoftDelete
{
    public bool IsDelete { get; set; }
}

public interface IHaveDateLog
{
    public string CreateDate { get; set; }
    public string LastUpdateDate { get; set; }
}