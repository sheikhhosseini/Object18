using Core.Modules.MemberModule.Dtos;
using Core.Modules.MemberModule.Services;
using Core.Shared.Paging;
using Core.Shared.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Object18.Areas.Admin.Controllers;

[Area("Admin")]
public class MemberController : Controller
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    // GET: MemberController
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
    public async Task<IActionResult> Create(MemberCreateDto input)
    {
        if (ModelState.IsValid)
        {
            var result = await _memberService.Create(input);
            if (result.Response == Core.Shared.Tools.Response.Success)
            {
                TempData["alert"] = "insertMemberDone";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                
            }
        }

        return View(input);
    }




    // GET: MemberController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: MemberController/Create
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create(IFormCollection collection)
    //{
    //    try
    //    {
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}

    // GET: MemberController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: MemberController/Edit/5
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