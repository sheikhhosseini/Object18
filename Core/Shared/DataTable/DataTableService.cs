using System.Linq.Dynamic.Core;
using AutoMapper;
using Core.Shared.Paging;
using Data.Models;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.DataTable;

public class DataTableService : IDataTableService
{
    private readonly IMapper _mapper;

    public DataTableService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<AdvanceDataTable<TDto>> GetDataTable<TEntity, TDto>(
        IQueryable<TEntity> query,
        AdvanceDataTable<TDto> dataTableRequest
    ) where TEntity : class, IHaveId where TDto : DataTableBaseDto
    {
        foreach (var filter in dataTableRequest.Filters)
        {
            if (filter.KeyType is "text" or "number" && !string.IsNullOrEmpty(filter.KeyValue.First()))
            {
                query = query.ApplyFiltering($"{filter.KeyName} {filter.KeyOperator} {filter.KeyValue.First()}");
            }
            else if (filter.KeyType == "date" && !string.IsNullOrEmpty(filter.KeyValue.First()))
            {
                query = query.Where("string.Compare(" + filter.KeyName + ", @0)" + filter.KeyOperator + "0", filter.KeyValue.First());
            }
            else if (filter.KeyType == "list" && filter.KeyValue.Count > 0 && !string.IsNullOrEmpty(filter.KeyValue.First()))
            {
                if (bool.TryParse(filter.KeyValue.First(), out _))
                {
                    List<bool> booleanValues = filter.KeyValue.ConvertAll(bool.Parse);

                    query = query
                        .Where($"@0.Contains({filter.KeyName})", booleanValues);
                }
                else
                {
                    List<long> ids = filter.KeyValue.ConvertAll(long.Parse);

                    switch (filter.KeyOperator)
                    {
                        case "=*":
                            query = query
                                .Include($"{filter.KeyName}")
                                .Where($"{filter.KeyName}.Any(@0.Contains(Id))", ids);
                            break;

                        case "!*":
                            query = query
                                .Include($"{filter.KeyName}")
                                .Where($"!{filter.KeyName}.Any(@0.Contains(Id))", ids);
                            break;
                    }
                }
            }
        }

        foreach (var sortOrder in dataTableRequest.SortOrder)
        {
            if (!string.IsNullOrEmpty(sortOrder.KeyName) && !string.IsNullOrEmpty(sortOrder.KeySort))
            {
                query = query.ApplyOrdering($"{sortOrder.KeyName} {sortOrder.KeySort}");
            }
        }

        return await GeneratePages(dataTableRequest, query);
    }

    private async Task<AdvanceDataTable<TDto>> GeneratePages<TEntity, TDto>(
        AdvanceDataTable<TDto> data,
        IQueryable<TEntity> query
    ) where TEntity : class, IHaveId where TDto : DataTableBaseDto
    {

        var pageCount = (int)Math.Ceiling(query.Count() / (double)data.TakeEntity);
        var pager = PageGenerator.Generate(pageCount, data.PageId, data.TakeEntity);
        var users = await query.Paging(pager).ToListAsync();

        var records = _mapper.Map<List<TDto>>(users);

        var rowNumber = pager.SkipEntity;
        foreach (var record in records)
        {
            record.Row = ++rowNumber;
        }

        var result = new AdvanceDataTable<TDto>
        {
            Records = records,
            PageId = pager.PageId,
            PageCount = pager.PageCount,
            StartPage = pager.StartPage,
            EndPage = pager.EndPage,
            TakeEntity = pager.TakeEntity,
            SkipEntity = pager.SkipEntity,
            ActivePage = pager.ActivePage,
        };

        return result;
    }
}