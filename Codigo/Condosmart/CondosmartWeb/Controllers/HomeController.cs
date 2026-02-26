using System.Diagnostics;
using Condosmart.Models;
using CondosmartWeb.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace Condosmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMensalidadeService _mensalidadeService;
        private readonly IVisitanteService _visitanteService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly IReservaService _reservaService;
        private readonly IAreaDeLazerService _areaService;

        public HomeController(
            ILogger<HomeController> logger,
            IMensalidadeService mensalidadeService,
            IVisitanteService visitanteService,
            IUnidadesResidenciaisService unidadesService,
            IReservaService reservaService,
            IAreaDeLazerService areaService)
        {
            _logger = logger;
            _mensalidadeService = mensalidadeService;
            _visitanteService = visitanteService;
            _unidadesService = unidadesService;
            _reservaService = reservaService;
            _areaService = areaService;
        }

        public IActionResult Index()
        {
            var hoje = DateTime.Today;
            var mensalidades = _mensalidadeService.GetAll();
            var visitantes = _visitanteService.GetAll();
            var unidades = _unidadesService.GetAll();
            var reservas = _reservaService.GetAll();
            var areas = _areaService.GetAll();

            var totalMensalidades = mensalidades.Count;
            var mensalidadesPagas = mensalidades
                .Where(m => m.Status == "pago" &&
                            m.Competencia.Month == hoje.Month &&
                            m.Competencia.Year == hoje.Year);
            var mensalidadesAtrasadas = mensalidades.Count(m => m.Status == "atrasado");

            var vm = new DashboardViewModel
            {
                TotalRecebidoMes      = mensalidadesPagas.Sum(m => m.Valor),
                TaxaInadimplencia     = totalMensalidades > 0
                                            ? Math.Round((double)mensalidadesAtrasadas / totalMensalidades * 100, 1)
                                            : 0,
                EntradasHoje          = visitantes.Count(v => v.DataHoraEntrada.HasValue &&
                                                               v.DataHoraEntrada.Value.Date == hoje),
                MensalidadesEmAtraso  = mensalidadesAtrasadas,
                UnidadesOcupadas      = unidades.Count(u => u.MoradorId != null),
                TotalUnidades         = unidades.Count,
                ReservasConfirmadas   = reservas.Count(r => r.Status == "confirmado" &&
                                                             r.DataFim.Date >= hoje),
                TotalAreasLazer       = areas.Count,
            };

            return View(vm);
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
