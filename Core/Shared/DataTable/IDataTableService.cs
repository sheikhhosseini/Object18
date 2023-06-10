using Core.Shared.Paging;
using Data.Models;

namespace Core.Shared.DataTable;

public interface IDataTableService
{
    Task<AdvanceDataTable<TDto>> GetDataTable<TEntity, TDto>(
        IQueryable<TEntity> query,
        AdvanceDataTable<TDto> dataTableRequest)
        where TEntity : class, IHaveId
        where TDto : DataTableBaseDto;
}