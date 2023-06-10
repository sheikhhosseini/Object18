using AutoMapper;
using Data.Models;

namespace Core.Modules.RoleModule.Dtos;

public class PermissionSelectItemDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string AreaName { get; set; }

    public string ModuleName { get; set; }

    public string ActionName { get; set; }
}

public class PermissionSelectItemProfile : Profile
{
    public PermissionSelectItemProfile()
    {
        CreateMap<Permission, PermissionSelectItemDto>(MemberList.Destination)
            .ForMember(dto => dto.AreaName, opt =>
                opt.MapFrom(model => model.PermissionArea.Name))

            .ForMember(dto => dto.ModuleName, opt =>
                opt.MapFrom(model => model.PermissionModule.Name))

            .ForMember(dto => dto.ActionName, opt =>
                opt.MapFrom(model => model.PermissionAction.Name));
    }
}