using System.Security.Claims;
using Core.Modules.Account.Dtos;
using Core.Modules.Account.Results;
using Core.Modules.Account.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Object18.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    #region Register
    [Route("Register")]
    public IActionResult Register()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterDto user)
    {
        if (ModelState.IsValid)
        {
            if (await _accountService.IsUserExist(user.Email))
            {
                ModelState.AddModelError("Email", "ایمیل مورد نظر در سایت موجود است");
                return View(user);
            }
            return View("SuccessRegister", await _accountService.RegisterUser(user, true));
        }
        return View(user);
    }
    #endregion

    #region ActiveAccount
    public async Task<IActionResult> ActiveAccount(string id)
    {
        return View(await _accountService.ActiveAccount(id));
    }
    #endregion

    #region EmailAjax
    [Route("EmailCheck")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EmailCheck(string email)
    {
        var result = await _accountService.IsUserExist(email);
        if (!result)
        {
            return Json(true);
        }
        return Json("ایمیل وارد شده در سایت موجود است!");
    }
    #endregion

    #region Login
    [Route("Login")]
    public IActionResult Login(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        ViewData["returnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginDto login, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            LoginResult result = await _accountService.LoginUser(login);

            switch (result.Status)
            {
                // cases 
                case LoginStatus.Success:
                    var claims = new List<Claim>
                    {
                        new (ClaimTypes.NameIdentifier,result.User.Id.ToString()),
                        new (ClaimTypes.Name,result.User.FullName),
                        new (ClaimTypes.Email,result.User.Email),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    await HttpContext.SignInAsync(principal, properties);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("index", "Home");
                case LoginStatus.NotActivated:
                    ModelState.AddModelError("Email", "حساب شما فعال نیست");
                    return View(login);
                case LoginStatus.Failed:
                    ModelState.AddModelError("Email", "اطلاعات وارد شده اشتباه است");
                    return View(login);
            }
        }

        return View(login);
    }
    #endregion

    #region Logout
    [Authorize]
    [Route("Logout")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/Login");
    }

    #endregion

    //#region ForgotPassword

    //[Route("ForgotPassword")]
    //public IActionResult ForgotPassword()
    //{
    //    if (User.Identity.IsAuthenticated)
    //    {
    //        return RedirectToAction("Index", "Home");
    //    }
    //    return View();
    //}


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //[Route("ForgotPassword")]
    //public async Task<IActionResult> ForgotPassword(ForgotPasswordDto data)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        if (await _userService.UserForgotPassword(data.Email))
    //        {
    //            ViewBag.message = "ایمیل بازیابی رمز عبور به نشانی  " + data.Email + "ارسال شد";
    //            ViewBag.disable = "disabled";
    //            return View(data);
    //        }
    //        ModelState.AddModelError("Email", "حساب کاربری یافت نشد");
    //        return View(data);
    //    }
    //    return View(data);
    //}
    //#endregion

    //#region ResetPassword

    //public IActionResult ResetPassword(string id)
    //{
    //    if (User.Identity.IsAuthenticated)
    //    {
    //        return RedirectToAction("Index", "Home");
    //    }
    //    var userDto = new ResetPasswordDto
    //    {
    //        ActiveCode = id
    //    };
    //    return View(userDto);
    //}


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> ResetPassword(ResetPasswordDto data)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        if (await _userService.UserResetPassword(data))
    //        {
    //            ViewBag.message = true;
    //            ViewBag.disable = "disabled";
    //            return View(data);
    //        }
    //        ModelState.AddModelError("Password", "خطا");
    //        return View(data);
    //    }
    //    return View(data);
    //}
    //#endregion
}