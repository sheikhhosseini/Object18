﻿using Core.Shared.Paging;

namespace Core.Modules.UserModule.Dtos;

public class Filter
{
    public string Name { get; set; }

    public string Value { get; set; }
}

public class Rule : BasePaging
{
    public List<Filter> Filters { get; set; }

    public string SortOrder { get; set; } = "asc";
}