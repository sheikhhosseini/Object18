using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.Account.Dtos;

public class RegisterDto
{
    [Display(Name = "نام")]
    [MaxLength(70, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string LastName { get; set; }

    [Remote("EmailCheck", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [EmailAddress(ErrorMessage = "لطفا {0} معتبر وارد کنید")]
    public string Email { get; set; }

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(20, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    public string Password { get; set; }

    [Display(Name = "تکرار رمز عبور")]
    [Required]
    [MaxLength(20)]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشد")]
    public string RePassword { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}