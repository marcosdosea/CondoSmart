using Core.Identity;
using CondosmartWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Morador)]
    public class MoradorDashboardController : Controller
    {
        private readonly IMoradorDashboardService _dashboardService;

        public MoradorDashboardController(IMoradorDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return Forbid();

            var viewModel = _dashboardService.Build(email);
            if (viewModel is null)
                return View("SemVinculo", email);

            return View(viewModel);
        }
    }
}
