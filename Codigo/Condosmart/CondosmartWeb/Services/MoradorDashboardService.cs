using System.Globalization;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;

namespace CondosmartWeb.Services
{
    public class MoradorDashboardService : IMoradorDashboardService
    {
        private static readonly CultureInfo CulturaBrasil = new("pt-BR");

        private readonly IMoradorService _moradorService;
        private readonly ICondominioService _condominioService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly IMensalidadeService _mensalidadeService;
        private readonly IAtaService _ataService;

        public MoradorDashboardService(
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

        public MoradorDashboardViewModel? Build(string email)
        {
            var morador = _moradorService.GetByEmail(email);
            if (morador is null)
                return null;

            var unidade = _unidadesService.GetByMoradorId(morador.Id);
            var condominioId = morador.CondominioId ?? unidade?.CondominioId;
            var condominio = condominioId.HasValue ? _condominioService.GetById(condominioId.Value) : null;

            var mensalidades = _mensalidadeService.GetByMorador(morador.Id)
                .Take(12)
                .ToList();

            var comunicados = condominioId.HasValue
                ? _ataService.GetAll()
                    .Where(a => a.CondominioId == condominioId.Value)
                    .OrderByDescending(a => a.DataReuniao)
                    .Take(5)
                    .ToList()
                : [];

            return new MoradorDashboardViewModel
            {
                MoradorId = morador.Id,
                Nome = morador.Nome,
                Email = morador.Email ?? email,
                Telefone = morador.Telefone,
                Condominio = condominio?.Nome ?? "Condominio nao informado",
                Unidade = unidade?.Identificador ?? "Unidade nao vinculada",
                MensalidadesPendentes = mensalidades.Count(m => !string.Equals(m.Status, "pago", StringComparison.OrdinalIgnoreCase)),
                Mensalidades = mensalidades.Select(MapMensalidade).ToList(),
                Comunicados = comunicados.Select(MapComunicado).ToList()
            };
        }

        private static MoradorMensalidadeResumoViewModel MapMensalidade(Mensalidade mensalidade)
        {
            return new MoradorMensalidadeResumoViewModel
            {
                Competencia = mensalidade.Competencia.ToString("MM/yyyy", CulturaBrasil),
                Valor = (mensalidade.ValorFinal > 0 ? mensalidade.ValorFinal : mensalidade.Valor).ToString("C", CulturaBrasil),
                Vencimento = mensalidade.Vencimento.ToString("dd/MM/yyyy", CulturaBrasil),
                Status = mensalidade.Status
            };
        }

        private static MoradorComunicadoResumoViewModel MapComunicado(Ata ata)
        {
            return new MoradorComunicadoResumoViewModel
            {
                Titulo = string.IsNullOrWhiteSpace(ata.Titulo) ? "Comunicado do condominio" : ata.Titulo,
                Data = ata.DataReuniao?.ToString("dd/MM/yyyy", CulturaBrasil) ?? "Sem data",
                Resumo = string.IsNullOrWhiteSpace(ata.Temas) ? "Sem resumo informado." : ata.Temas
            };
        }
    }
}
