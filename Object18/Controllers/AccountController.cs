using Core.Modules.Account.Dtos;
using Core.Modules.Account.ResultDtos;
using Core.Modules.Account.Services;
using Microsoft.AspNetCore.Mvc;

namespace Object18.Controllers
{
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
                return View("SuccessRegister", await _accountService.RegisterUser(user,true));
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
    }
}
