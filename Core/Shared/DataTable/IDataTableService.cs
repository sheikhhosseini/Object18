using AngleSharp.Dom;
using Core.Modules.MemberModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Data.Models;

namespace Core.Shared.DataTable;

public interface IDataTableService
{
    Task<AdvanceDataTable<TDto>> GetDataTable<TEntity, TDto>(
        IQueryable<TEntity> query,
        AdvanceDataTable<TDto> dataTableRequest)
        where TEntity : BaseModel
        where TDto : DataTableBaseDto;
}