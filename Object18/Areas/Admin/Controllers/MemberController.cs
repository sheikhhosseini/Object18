using Core.Modules.MemberModule.Dtos;
using Core.Modules.MemberModule.Services;
using Core.Shared.Paging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Test1(string data)
    {
        var result = new List<Sample>
        {
            new()
            {
                Id = 1,
                Text = "aaa"
            },
            new()
            {
                Id = 2,
                Text = "bbb"
            }
        };
        return new JsonResult(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> GridAjax([FromBody] AdvanceDataTable<MemberDataTableDto> data)
    {
        var result = await _memberService.GetDataTable(data);
        return new JsonResult(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] List<long> ids)
    {
        return Json(await _memberService.Delete(ids));
    }


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


    public ActionResult Create()
    {
        return View();
    }

    public async Task<ActionResult> Edit(int id)
    {
        return View(await _memberService.Get(id));
    }

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
}