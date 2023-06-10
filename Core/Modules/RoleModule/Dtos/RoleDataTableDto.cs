using AutoMapper;
using Core.Shared.DataTable;
using Data.Models;

namespace Core.Modules.RoleModule.Dtos;

public class RoleDataTableDto : DataTableBaseDto
{
    public long Id { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}

public class RoleDataTableDtoProfile : Profile
{
    public RoleDataTableDtoProfile()
    {
        CreateMap<Role, RoleDataTableDto>(MemberList.Destination).ReverseMap();
    }
}