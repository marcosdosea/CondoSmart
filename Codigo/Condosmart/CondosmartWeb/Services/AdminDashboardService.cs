using CondosmartWeb.Models;
using Core.Service;

namespace CondosmartWeb.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IMensalidadeService _mensalidadeService;
        private readonly IVisitanteService _visitanteService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly IReservaService _reservaService;
        private readonly IAreaDeLazerService _areaService;

        public AdminDashboardService(
            IMensalidadeService mensalidadeService,
            IVisitanteService visitanteService,
            IUnidadesResidenciaisService unidadesService,
            IReservaService reservaService,
            IAreaDeLazerService areaService)
        {
            _mensalidadeService = mensalidadeService;
            _visitanteService = visitanteService;
            _unidadesService = unidadesService;
            _reservaService = reservaService;
            _areaService = areaService;
        }

        public DashboardViewModel Build()
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
                            m.Competencia.Year == hoje.Year)
                .ToList();
            var mensalidadesAtrasadas = mensalidades.Count(m => m.Status == "atrasado");

            return new DashboardViewModel
            {
                TotalRecebidoMes = mensalidadesPagas.Sum(m => m.ValorFinal > 0 ? m.ValorFinal : m.Valor),
                TaxaInadimplencia = totalMensalidades > 0
                    ? Math.Round((double)mensalidadesAtrasadas / totalMensalidades * 100, 1)
                    : 0,
                EntradasHoje = visitantes.Count(v => v.DataHoraEntrada.HasValue && v.DataHoraEntrada.Value.Date == hoje),
                MensalidadesEmAtraso = mensalidadesAtrasadas,
                UnidadesOcupadas = unidades.Count(u => u.MoradorId != null),
                TotalUnidades = unidades.Count,
                ReservasConfirmadas = reservas.Count(r => r.Status == "confirmado" && r.DataFim.Date >= hoje),
                TotalAreasLazer = areas.Count,
                MensalidadesAbertas = mensalidades
                    .Where(m => m.Status != "pago")
                    .OrderBy(m => m.Vencimento)
                    .Take(5)
                    .Select(m => new DashboardMensalidadeItemViewModel
                    {
                        Id = m.Id,
                        Unidade = m.Unidade?.Identificador ?? $"Unidade {m.UnidadeId}",
                        Morador = m.Morador?.Nome ?? "-",
                        Vencimento = m.Vencimento,
                        Valor = m.ValorFinal > 0 ? m.ValorFinal : m.Valor,
                        Status = m.Status
                    })
                    .ToList()
            };
        }
    }
}
