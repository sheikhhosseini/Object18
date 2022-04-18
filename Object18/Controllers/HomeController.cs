using Microsoft.AspNetCore.Mvc;
using Object18.Models;
using System.Diagnostics;
using System.Reflection;
using Core.Modules.Account.Services;

namespace Object18.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAccountService _accountService;

    public HomeController(ILogger<HomeController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
