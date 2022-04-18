using Microsoft.AspNetCore.Mvc;

namespace Object18.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        //return Content("Admin Area");
        return View();
    }
}