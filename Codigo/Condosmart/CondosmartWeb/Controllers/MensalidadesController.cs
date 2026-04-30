using AutoMapper;
using CondosmartWeb.Models;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class MensalidadesController : Controller
    {
        private readonly IMensalidadeService _mensalidadeService;
        private readonly ICondominioService _condominioService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly IMapper _mapper;

        public MensalidadesController(
            IMensalidadeService mensalidadeService,
            ICondominioService condominioService,
            IUnidadesResidenciaisService unidadesService,
            IMapper mapper)
        {
            _mensalidadeService = mensalidadeService;
            _condominioService = condominioService;
            _unidadesService = unidadesService;
            _mapper = mapper;
        }

        public IActionResult Index(FiltroMensalidadeViewModel filtro)
        {
            var viewModel = MontarPagina(filtro);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarConfiguracao(ConfiguracaoMensalidadeViewModel configuracaoVm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", MontarPagina(new FiltroMensalidadeViewModel(), configuracaoVm, null));

                var configuracao = new ConfiguracaoMensalidade
                {
                    CondominioId = configuracaoVm.CondominioId,
                    ValorMensalidade = configuracaoVm.ValorMensalidade,
                    DiaVencimento = configuracaoVm.DiaVencimento,
                    QuantidadeParcelasPadrao = configuracaoVm.QuantidadeParcelasPadrao,
                    Ativa = configuracaoVm.Ativa
                };

                _mensalidadeService.SalvarConfiguracao(configuracao);
                TempData["Sucesso"] = "Configuracao de mensalidade salva com sucesso.";
                return RedirectToAction(nameof(Index), new { condominioId = configuracaoVm.CondominioId, page = 1 });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", MontarPagina(new FiltroMensalidadeViewModel(), configuracaoVm, null));
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel salvar a configuracao de mensalidade agora.";
                return View("Index", MontarPagina(new FiltroMensalidadeViewModel(), configuracaoVm, null));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GerarParcelas(GerarParcelasMensalidadeViewModel geracaoVm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", MontarPagina(new FiltroMensalidadeViewModel(), null, geracaoVm));

                var resultado = _mensalidadeService.GerarParcelasEmLote(
                    geracaoVm.CondominioId,
                    geracaoVm.AnoReferencia,
                    geracaoVm.QuantidadeParcelas);

                TempData["Sucesso"] = $"Geracao concluida: {resultado.ParcelasGeradas} parcela(s) criada(s) e {resultado.ParcelasIgnoradas} ignorada(s).";
                return RedirectToAction(nameof(Index), new
                {
                    condominioId = geracaoVm.CondominioId,
                    anoCompetencia = geracaoVm.AnoReferencia,
                    page = 1
                });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", MontarPagina(new FiltroMensalidadeViewModel(), null, geracaoVm));
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel gerar as parcelas agora.";
                return View("Index", MontarPagina(new FiltroMensalidadeViewModel(), null, geracaoVm));
            }
        }

        public IActionResult Details(int id)
        {
            var mensalidade = _mensalidadeService.GetById(id);
            if (mensalidade == null)
                return NotFound();

            return View(_mapper.Map<MensalidadeViewModel>(mensalidade));
        }

        public IActionResult Pagar(int id)
        {
            TempData["Erro"] = "Pagamento ainda nao foi implementado nesta etapa. TODO: implementar pagamento.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pagar(PagarMensalidadeViewModel pagarVm)
        {
            TempData["Erro"] = "Pagamento ainda nao foi implementado nesta etapa. TODO: implementar pagamento.";
            return RedirectToAction(nameof(Details), new { id = pagarVm.MensalidadeId });
        }

        public IActionResult Comprovante(int id)
        {
            TempData["Erro"] = "Comprovante indisponivel enquanto o fluxo de pagamento nao for implementado.";
            return RedirectToAction(nameof(Details), new { id });
        }

        private MensalidadesAdminPageViewModel MontarPagina(
            FiltroMensalidadeViewModel filtro,
            ConfiguracaoMensalidadeViewModel? configuracao = null,
            GerarParcelasMensalidadeViewModel? geracao = null)
        {
            filtro ??= new FiltroMensalidadeViewModel();

            var mensalidades = _mensalidadeService.Filtrar(
                filtro.CondominioId,
                filtro.UnidadeId,
                filtro.Status,
                filtro.MesCompetencia,
                filtro.AnoCompetencia);

            var condominios = _condominioService.GetAll()
                .OrderBy(c => c.Nome)
                .ToList();

            var unidades = _unidadesService.GetAll()
                .Where(u => !filtro.CondominioId.HasValue || u.CondominioId == filtro.CondominioId.Value)
                .OrderBy(u => u.Condominio?.Nome)
                .ThenBy(u => u.Identificador)
                .ToList();

            var configuracoes = _mensalidadeService.GetConfiguracoes();

            return new MensalidadesAdminPageViewModel
            {
                Filtro = filtro,
                Configuracao = configuracao ?? new ConfiguracaoMensalidadeViewModel
                {
                    CondominioId = filtro.CondominioId ?? 0
                },
                Geracao = geracao ?? new GerarParcelasMensalidadeViewModel
                {
                    CondominioId = filtro.CondominioId ?? 0,
                    AnoReferencia = filtro.AnoCompetencia ?? DateTime.Today.Year
                },
                Mensalidades = PagedListViewModel<MensalidadeViewModel>.Create(
                    _mapper.Map<List<MensalidadeViewModel>>(mensalidades),
                    filtro.Page,
                    filtro.PageSize),
                Configuracoes = configuracoes.Select(c => new ConfiguracaoMensalidadeResumoViewModel
                {
                    Condominio = c.Condominio.Nome,
                    ValorMensalidade = c.ValorMensalidade,
                    DiaVencimento = c.DiaVencimento,
                    QuantidadeParcelasPadrao = c.QuantidadeParcelasPadrao,
                    Ativa = c.Ativa
                }).ToList(),
                Condominios = condominios.Select(c => new SelectListItem(c.Nome, c.Id.ToString())).ToList(),
                Unidades = unidades.Select(u => new SelectListItem(
                    $"{u.Condominio?.Nome} - {u.Identificador}",
                    u.Id.ToString())).ToList()
            };
        }
    }
}
