namespace Core.Shared.Paging;

public class AdvanceDataTable<TDataTableDto> : BasePaging
{
    public List<TDataTableDto> Records { get; set; }

    public List<Filter> Filters { get; set; }

    public string SortOrder { get; set; } = "asc";
}

public class Filter
{
    public string KeyName { get; set; }

    public string KeyOperator { get; set; }

    public string KeyValue { get; set; }
}