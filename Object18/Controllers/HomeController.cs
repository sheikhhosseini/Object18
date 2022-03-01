using Microsoft.AspNetCore.Mvc;
using Object18.Models;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Object18.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var appDomain = AppDomain.CurrentDomain;
            //var assemblies = appDomain.GetAssemblies();

            //foreach (var assembly in assemblies)
            //{
            //    var types = assembly.GetTypes();

            //    foreach (var type in types)
            //    {
            //        var interfaces = type.GetInterfaces();

            //        foreach (var iface in interfaces)
            //        {
            //            if (iface.IsGenericType)
            //            {
            //                var genericInterface = iface.GetGenericTypeDefinition();

            //                if (genericInterface == typeof(MyGenericInterface<>))
            //                {
            //                    Console.WriteLine($"Type {type.Name} implements {genericInterface.Name}");
            //                }
            //            }
            //        }
            //    }
            //}

           

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}