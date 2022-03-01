using Microsoft.AspNetCore.Mvc;
using Object18.Models;
using System.Diagnostics;
using System.Reflection;
using Core.Modules.TestModule.Dtos;
using Core.Modules.TestModule.Services;
using Microsoft.EntityFrameworkCore;

namespace Object18.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITestService _testService;

        public HomeController(ILogger<HomeController> logger, ITestService testService)
        {
            _logger = logger;
            _testService = testService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var createDto = new RoleCreateDto
            {
                RoleDescription = "xxx",
                RoleTitle = "11"
            };
            await _testService.AddTest(createDto);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}