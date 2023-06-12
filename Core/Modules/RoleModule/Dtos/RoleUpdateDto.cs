using AutoMapper;
using Data.Models;

namespace Core.Modules.RoleModule.Dtos;

public class RoleUpdateDto : RoleCreateDto
{
    public long Id { get; set; }

    public string ConcurrencyStamp { get; set; }
}

public class RoleUpdateDtoProfile : Profile
{
    public RoleUpdateDtoProfile()
    {
        CreateMap<Role, RoleUpdateDto>(MemberList.Destination)
            .ForMember(role => role.PermissionIds, opt =>
                opt.MapFrom(role => role.RolePermissions.Select(x => x.PermissionId)))
            .ReverseMap()
            .ForMember(model => model.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(model => model.RolePermissions, opt => opt.Ignore());
    }
}