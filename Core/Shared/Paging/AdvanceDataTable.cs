namespace Core.Shared.Paging;

public class AdvanceDataTable<TDataTableDto> : BasePaging
{
    public List<TDataTableDto> Records { get; set; }

    public List<Filter> Filters { get; set; }

    public List<SortOrder> SortOrder { get; set; }
}

public class Filter
{
    public string KeyName { get; set; }

    public string KeyOperator { get; set; }

    public string KeyType { get; set; }

    public List<string> KeyValue { get; set; }
}

public class SortOrder
{
    public string KeyName { get; set; }

    public string KeySort { get; set; }
}