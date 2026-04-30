using System.Diagnostics;
using Condosmart.Models;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Identity;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condosmart.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminDashboardService _dashboardService;

        public HomeController(
            ILogger<HomeController> logger,
            IAdminDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View(_dashboardService.Build());
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
