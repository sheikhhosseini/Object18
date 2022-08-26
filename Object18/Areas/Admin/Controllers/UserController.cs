using Core.Modules.UserModule.Dtos;
using Core.Modules.UserModule.Services;
using Core.Shared.Paging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Object18.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: UserController
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> GridAjax([FromBody] AdvanceDataTable<UserDataTableDto> data)
    {
        var result = await _userService.GetDataTable(data);
        return new JsonResult(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] List<long> ids)
    {
        return Json(await _userService.Delete(ids));
    }














    // GET: UserController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UserController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: UserController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: UserController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}