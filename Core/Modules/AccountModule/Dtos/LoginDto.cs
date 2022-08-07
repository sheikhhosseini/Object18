using System.ComponentModel.DataAnnotations;

namespace Core.Modules.AccountModule.Dtos;

public class LoginDto
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    [EmailAddress(ErrorMessage = "لطفا {0} معتبر وارد کنید")]
    public string Email { get; set; }


    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(20, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
    public string Password { get; set; }


    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
}