using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Shared.Tools;
using Data.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Modules.MemberModule.Dtos;

public class MemberCreateDto
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
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string MobileNumber { get; set; }

    [Display(Name = "کد ملی")]
    [MaxLength(10, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string KodMeli { get; set; }

    [Display(Name = "تاریخ تولد")]
    [MaxLength(10, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string DateOfBirth { get; set; }

    [Display(Name = "تاریخ ثبت نام")]
    [MaxLength(10, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string RegisterDate { get; set; }

    [Display(Name = "ایمیل")]
    [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [EmailAddress(ErrorMessage = "لطفا {0} معتبر وارد کنید")]
    public string Email { get; set; }

    [Display(Name = "جنسیت")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public bool Gender { get; set; }

    [Display(Name = "آدرس")]
    [MaxLength(400, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    public string Address { get; set; }

    public IFormFile ImageFile { get; set; }
}

public class MemberCreateDtoProfile : Profile
{
    public MemberCreateDtoProfile()
    {
        CreateMap<MemberCreateDto, Member>(MemberList.Destination)
            .ForMember(model => model.Image, opt =>
                opt.Ignore())
            .ForMember(model => model.FirstName, opt =>
                opt.MapFrom(dto => dto.FirstName.SanitizeText()))
            .ForMember(model => model.LastName, opt =>
                opt.MapFrom(dto => dto.LastName.SanitizeText()))
            .ForMember(model => model.Email, opt =>
                opt.MapFrom(dto => dto.Email.SanitizeText()));
    }
}