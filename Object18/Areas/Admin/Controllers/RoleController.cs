using Core.Modules.RoleModule.Dtos;
using Core.Modules.RoleModule.Services;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Object18.Areas.Admin.Controllers;

[Area("Admin")]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public ActionResult Index()
    {
        return View();
    }

    //[Permission(RoleControllerPermissions.List)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> GetDataTable([FromBody] AdvanceDataTable<RoleDataTableDto> data)
    {
        return new JsonResult(await _roleService.GetDataTable(data));
    }

    public ActionResult Create()
    {
        return View();
    }

    //[Permission(RoleControllerPermissions.Create)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleCreateDto createDto)
    {
        if (!ModelState.IsValid) return View(createDto);

        var result = await _roleService.Create(createDto);

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
        return View(await _roleService.Get(id));
    }

    //[Permission(RoleControllerPermissions.Update)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RoleUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return View(updateDto);

        var result = await _roleService.Update(updateDto);

        TempData["Response"] = result.Response;
        TempData["AlertMessage"] = result.Message;

        if (result.Response == Core.Shared.Tools.Response.Success)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(updateDto);
    }

    //[Permission(RoleControllerPermissions.Delete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] List<RoleDeleteDto> deleteDtos)
    {
        return Json(await _roleService.Delete(deleteDtos));
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

    [HttpGet]
    public async Task<ActionResult> GetPermissionList()
    {
        return new JsonResult(await _roleService.GetPermissionList());
    }
}