﻿using Core.Modules.UserModule.Dtos;
using Core.Modules.UserModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult> Index()
    {
        var result = await _userService.GetDataTable(null);
        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> GridAjax([FromBody] List<Filter> data)
    {
        var result = await _userService.GetDataTable(data);
        return new JsonResult(result);
    }

    











    
    // GET: UserController/Details/5
    public ActionResult Details(int id)
    {
        return View();
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

    // GET: UserController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: UserController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
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