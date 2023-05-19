using Core.Modules.MemberModule.Dtos;
using Core.Shared.Paging;
using Core.Shared.Tools;

namespace Core.Modules.MemberModule.Services;

public interface IMemberService
{
    Task<AdvanceDataTable<MemberDataTableDto>> GetDataTable(AdvanceDataTable<MemberDataTableDto> data);

    Task<OperationResult<MemberUpdateDto>> Create(MemberCreateDto createDto);
    
    Task<MemberUpdateDto> Update(MemberUpdateDto updateDto);

    Task<MemberUpdateDto> Get(long id);

    Task<OperationResult<MemberUpdateDto>> Delete(List<long> deleteDtos);
}