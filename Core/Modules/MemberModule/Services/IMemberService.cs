using Core.Modules.MemberModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;

namespace Core.Modules.MemberModule.Services;

public interface IMemberService
{
    Task<AdvanceDataTable<MemberDataTableDto>> GetDataTable(AdvanceDataTable<MemberDataTableDto> dataTableRequest);

    Task<OperationResult<MemberUpdateDto>> Create(MemberCreateDto createDto);

    Task<MemberUpdateDto> Get(long id);

    Task<OperationResult<MemberUpdateDto>> Update(MemberUpdateDto updateDto);

    Task<OperationResult<MemberUpdateDto>> Delete(List<MemberDeleteDto> deleteDtos);

    Task<List<SelectItemDto>> SelectItems();
}