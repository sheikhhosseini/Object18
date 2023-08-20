using Core.Modules.UserModule.Dtos;
using Core.Modules.UserModule.Services;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Microsoft.AspNetCore.Mvc;
using Object18.Controllers;
using Object18.PermissionChecker;

namespace Object18.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public ActionResult Index()
    {
        return View();
    }

    //[Permission(UserControllerPermissions.List)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> GetDataTable([FromBody] AdvanceDataTable<UserDataTableDto> data)
    {
        return new JsonResult(await _userService.GetDataTable(data));
    }

    public ActionResult Create()
    {
        return View();
    }

    //[Permission(UserControllerPermissions.Create)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateDto createDto)
    {
        if (!ModelState.IsValid) return View(createDto);

        var result = await _userService.Create(createDto);

        TempData["Response"] = result.Response;
        TempData["AlertMessage"] = result.Message;

        if (result.Response == Core.Shared.Tools.Response.Success)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(createDto);
    }

    public async Task<ActionResult> Edit(int id)
    {
        return View(await _userService.Get(id));
    }

    //[Permission(UserControllerPermissions.Update)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return View(updateDto);

        var result = await _userService.Update(updateDto);

        TempData["Response"] = result.Response;
        TempData["AlertMessage"] = result.Message;

        if (result.Response == Core.Shared.Tools.Response.Success)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(updateDto);
    }

    //[Permission(UserControllerPermissions.Delete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] List<UserDeleteDto> deleteDtos)
    {
        return Json(await _userService.Delete(deleteDtos));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult GetGenderDataSource()
    {
        var result = new List<SelectItemDto>
        {
            new()
            {
                Text = "مرد",
                Id = "true"
            },
            new()
            {
                Text = "زن",
                Id = "false"
            }
        };
        return new JsonResult(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> GetMissionDataSource()
    {
        var result = await _userService.SelectItems();
        return new JsonResult(result);
    }
}