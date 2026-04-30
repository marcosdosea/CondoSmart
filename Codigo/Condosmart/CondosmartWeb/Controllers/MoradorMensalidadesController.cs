using AutoMapper;
using CondosmartWeb.Models;
using Core.Identity;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Morador)]
    public class MoradorMensalidadesController : Controller
    {
        private readonly IMoradorService _moradorService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly ICondominioService _condominioService;
        private readonly IMensalidadeService _mensalidadeService;
        private readonly IMapper _mapper;

        public MoradorMensalidadesController(
            IMoradorService moradorService,
            IUnidadesResidenciaisService unidadesService,
            ICondominioService condominioService,
            IMensalidadeService mensalidadeService,
            IMapper mapper)
        {
            _moradorService = moradorService;
            _unidadesService = unidadesService;
            _condominioService = condominioService;
            _mensalidadeService = mensalidadeService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return Forbid();

            var morador = _moradorService.GetByEmail(email);

            if (morador is null)
                return RedirectToAction("Index", "MoradorDashboard");

            var unidade = _unidadesService.GetByMoradorId(morador.Id);

            var condominioId = morador.CondominioId ?? unidade?.CondominioId;
            var condominio = condominioId.HasValue ? _condominioService.GetById(condominioId.Value) : null;

            var mensalidades = _mensalidadeService.GetByMorador(morador.Id);

            var viewModel = new MoradorMensalidadesPageViewModel
            {
                NomeMorador = morador.Nome,
                Condominio = condominio?.Nome ?? "Condominio nao informado",
                Unidade = unidade?.Identificador ?? "Unidade nao vinculada",
                Mensalidades = _mapper.Map<List<MensalidadeViewModel>>(mensalidades)
            };

            return View(viewModel);
        }
    }
}
