using FleetManagementService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FleetManagementService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        // Call Home Page
        public IActionResult Index()
        {
            return View();
        }


        // Call Vessel Page
        public IActionResult VesselIndex()
        {
            return View();
        }

        // Call Containers Page
        public IActionResult ContainersIndex()
        {
            return View();
        }

        // Call Fleets Page
        public IActionResult FleetsIndex()
        {
            return View();
        }

        // Call Transfer Page
        public IActionResult TransferIndex()
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
