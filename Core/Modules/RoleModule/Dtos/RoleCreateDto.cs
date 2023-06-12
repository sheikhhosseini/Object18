using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Models;

namespace Core.Modules.RoleModule.Dtos;

public class RoleCreateDto
{
    [Display(Name = "نام")]
    [MaxLength(70, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Name { get; set; }

    [Display(Name = "توضیحات")]
    public string Description { get; set; }

    public List<long> PermissionIds { get; set; } = new();
}

public class RoleCreateDtoProfile : Profile
{
    public RoleCreateDtoProfile()
    {
        CreateMap<RoleCreateDto, Role>(MemberList.Destination)
            .ForMember(role => role.RolePermissions, opt =>
                opt.MapFrom(dto => dto.PermissionIds.Select(
                    permissionId =>
                        new RolePermission
                        {
                            PermissionId = permissionId
                        }).ToList()));
    }
}