using System.Globalization;
using CondosmartWeb.Models;
using Core.Identity;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Morador)]
    public class MoradorDashboardController : Controller
    {
        private static readonly CultureInfo CulturaBrasil = new("pt-BR");

        private readonly IMoradorService _moradorService;
        private readonly ICondominioService _condominioService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly IMensalidadeService _mensalidadeService;
        private readonly IAtaService _ataService;

        public MoradorDashboardController(
            IMoradorService moradorService,
            ICondominioService condominioService,
            IUnidadesResidenciaisService unidadesService,
            IMensalidadeService mensalidadeService,
            IAtaService ataService)
        {
            _moradorService = moradorService;
            _condominioService = condominioService;
            _unidadesService = unidadesService;
            _mensalidadeService = mensalidadeService;
            _ataService = ataService;
        }

        public IActionResult Index()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return Forbid();

            var morador = _moradorService.GetAll()
                .FirstOrDefault(m => string.Equals(m.Email, email, StringComparison.OrdinalIgnoreCase));

            if (morador is null)
                return View("SemVinculo", email);

            var unidade = _unidadesService.GetAll()
                .FirstOrDefault(u => u.MoradorId == morador.Id);

            var condominioId = morador.CondominioId ?? unidade?.CondominioId;
            var condominio = condominioId.HasValue ? _condominioService.GetById(condominioId.Value) : null;

            var mensalidades = _mensalidadeService.GetAll()
                .Where(m => m.MoradorId == morador.Id)
                .OrderByDescending(m => m.Competencia)
                .Take(12)
                .ToList();

            var comunicados = condominioId.HasValue
                ? _ataService.GetAll()
                    .Where(a => a.CondominioId == condominioId.Value)
                    .OrderByDescending(a => a.DataReuniao)
                    .Take(5)
                    .ToList()
                : new List<Core.Models.Ata>();

            var viewModel = new MoradorDashboardViewModel
            {
                MoradorId = morador.Id,
                Nome = morador.Nome,
                Email = morador.Email ?? email,
                Telefone = morador.Telefone,
                Condominio = condominio?.Nome ?? "Condomínio não informado",
                Unidade = unidade?.Identificador ?? "Unidade não vinculada",
                MensalidadesPendentes = mensalidades.Count(m => !string.Equals(m.Status, "Pago", StringComparison.OrdinalIgnoreCase)),
                Mensalidades = mensalidades.Select(m => new MoradorMensalidadeResumoViewModel
                {
                    Competencia = m.Competencia.ToString("MM/yyyy", CulturaBrasil),
                    Valor = m.Valor.ToString("C", CulturaBrasil),
                    Vencimento = m.Vencimento.ToString("dd/MM/yyyy", CulturaBrasil),
                    Status = m.Status
                }).ToList(),
                Comunicados = comunicados.Select(a => new MoradorComunicadoResumoViewModel
                {
                    Titulo = string.IsNullOrWhiteSpace(a.Titulo) ? "Comunicado do condomínio" : a.Titulo,
                    Data = a.DataReuniao?.ToString("dd/MM/yyyy", CulturaBrasil) ?? "Sem data",
                    Resumo = string.IsNullOrWhiteSpace(a.Temas) ? "Sem resumo informado." : a.Temas
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
