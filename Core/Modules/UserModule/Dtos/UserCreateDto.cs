using AutoMapper;
using Core.Shared.Tools;
using Data.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Modules.UserModule.Dtos;

public class UserCreateDto
{
    [Display(Name = "نام")]
    [MaxLength(70, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [MaxLength(120, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string LastName { get; set; }

    [Display(Name = "تلفن همراه")]
    [MaxLength(11, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    public string MobileNumber { get; set; }

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [EmailAddress(ErrorMessage = "لطفا {0} معتبر وارد کنید")]
    public string Email { get; set; }

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(20, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    public string Password { get; set; }

    public string ActiveCode { get; set; }

    public bool IsActive { get; set; }

    public IFormFile UserImage { get; set; }

    public ICollection<UserRoleDto> UserRoles { get; set; }
}

public class UserCreateDtoProfile : Profile
{
    public UserCreateDtoProfile()
    {
        CreateMap<UserCreateDto, User>(MemberList.Destination)
            .ForMember(model => model.UserImage, opt =>
                opt.Ignore())
            .ForMember(model => model.FirstName, opt =>
                opt.MapFrom(dto => dto.FirstName.SanitizeText()))
            .ForMember(model => model.LastName, opt =>
                opt.MapFrom(dto => dto.LastName.SanitizeText()))
            .ForMember(model => model.Email, opt =>
                opt.MapFrom(dto => dto.Email.SanitizeText()))
            .ForMember(model => model.Password, opt =>
                opt.MapFrom(dto => dto.Password.EncodePasswordMd5()));
    }
}