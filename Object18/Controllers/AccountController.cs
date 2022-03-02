using Core.Modules.Account.Dtos;
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
        public async Task<IActionResult>  Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var dto = new RegisterDto
            {
                Email = "a@a.com",
                FirstName = "ali",
                LastName = "saeedi",
                Password = "123"
            };
            await _accountService.RegisterUser(dto);

            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("Register")]
        //public async Task<IActionResult> Register(RegisterDto user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (await _userService.IsUserExistByEmail(user.Email))
        //        {
        //            ModelState.AddModelError("Email", "ایمیل مورد نظر در سایت موجود است");
        //            return View(user);
        //        }

        //        return View("SuccessRegister", await _userService.RegisterUser(user));
        //    }
        //    return View(user);
        //}
        #endregion
    }
}
