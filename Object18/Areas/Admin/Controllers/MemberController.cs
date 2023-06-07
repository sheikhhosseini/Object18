using Core.Modules.MemberModule.Dtos;
using Core.Modules.MemberModule.Services;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Microsoft.AspNetCore.Mvc;
using Object18.Controllers;
using Object18.PermissionChecker;

namespace Object18.Areas.Admin.Controllers;

[Area("Admin")]
public class MemberController : Controller
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public ActionResult Index()
    {
        return View();
    }

    //[Permission(MemberControllerPermissions.List)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> GetDataTable([FromBody] AdvanceDataTable<MemberDataTableDto> data)
    {
        return new JsonResult(await _memberService.GetDataTable(data));
    }

    public ActionResult Create()
    {
        return View();
    }

    //[Permission(MemberControllerPermissions.Create)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberCreateDto createDto)
    {
        if (!ModelState.IsValid) return View(createDto);

        var result = await _memberService.Create(createDto);

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
        return View(await _memberService.Get(id));
    }

    //[Permission(MemberControllerPermissions.Update)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MemberUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return View(updateDto);

        var result = await _memberService.Update(updateDto);

        TempData["Response"] = result.Response;
        TempData["AlertMessage"] = result.Message;

        if (result.Response == Core.Shared.Tools.Response.Success)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(updateDto);
    }

    //[Permission(MemberControllerPermissions.Delete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] List<MemberDeleteDto> deleteDtos)
    {
        return Json(await _memberService.Delete(deleteDtos));
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
        var result = await _memberService.SelectItems();
        return new JsonResult(result);
    }
}