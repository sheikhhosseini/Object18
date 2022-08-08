namespace Core.Shared.Paging;

public class AdvanceDataTable<TDataTableDto> : BasePaging
{
    public List<TDataTableDto> Records { get; set; }

    public List<Filter> Filters { get; set; }

    public string SortOrder { get; set; } = "asc";
}

public class Filter
{
    public string Name { get; set; }

    public string Value { get; set; }
}