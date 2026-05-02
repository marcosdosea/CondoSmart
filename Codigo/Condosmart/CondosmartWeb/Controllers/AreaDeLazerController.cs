using AutoMapper;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class AreaDeLazerController : Controller
    {
        private readonly IAreaDeLazerService _service;
        private readonly ICondominioService _condominioService;
        private readonly ISindicoService _sindicoService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly IArquivoUploadService _arquivoUploadService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public AreaDeLazerController(
            IAreaDeLazerService service,
            ICondominioService condominioService,
            ISindicoService sindicoService,
            ICondominioContextService condominioContextService,
            IArquivoUploadService arquivoUploadService,
            INotificacaoService notificacaoService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _sindicoService = sindicoService;
            _condominioContextService = condominioContextService;
            _arquivoUploadService = arquivoUploadService;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            var lista = _service.GetAll()
                .Where(a => !condominioAtualId.HasValue || a.CondominioId == condominioAtualId.Value)
                .OrderBy(a => a.Nome)
                .ToList();
            var listaVm = _mapper.Map<List<AreaDeLazerViewModel>>(lista);
            return View(listaVm);
        }

        public ActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<AreaDeLazerViewModel>(entity));
        }

        public ActionResult Create()
        {
            var vm = new AreaDeLazerViewModel
            {
                CondominioId = _condominioContextService.GetCondominioAtualId() ?? 0,
                Disponibilidade = true
            };
            PopularDropdowns(vm.CondominioId, vm.SindicoId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AreaDeLazerViewModel areaVm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns(areaVm.CondominioId, areaVm.SindicoId);
                return View(areaVm);
            }

            try
            {
                var area = _mapper.Map<AreaDeLazer>(areaVm);
                area.ImagemNomeOriginal = areaVm.ImagemNomeOriginal;
                area.ImagemCaminho = areaVm.ImagemCaminho;

                if (areaVm.ImagemArquivo is not null && areaVm.ImagemArquivo.Length > 0)
                {
                    var arquivo = await _arquivoUploadService.SalvarAsync(areaVm.ImagemArquivo, "areas-lazer");
                    area.ImagemNomeOriginal = arquivo.arquivoNomeOriginal;
                    area.ImagemCaminho = arquivo.arquivoCaminho;
                }

                var id = _service.Create(area);
                TempData["Sucesso"] = "Area de lazer cadastrada com sucesso.";
                RegistrarNotificacao(
                    area.CondominioId,
                    "Area de lazer cadastrada",
                    $"A area \"{area.Nome}\" foi cadastrada.",
                    Url.Action(nameof(Details), new { id }) ?? Url.Action(nameof(Index)) ?? "/AreaDeLazer");

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel cadastrar a area de lazer agora.";
            }

            PopularDropdowns(areaVm.CondominioId, areaVm.SindicoId);
            return View(areaVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var vm = _mapper.Map<AreaDeLazerViewModel>(item);
            PopularDropdowns(vm.CondominioId, vm.SindicoId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AreaDeLazerViewModel areaVm)
        {
            if (id != areaVm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopularDropdowns(areaVm.CondominioId, areaVm.SindicoId);
                return View(areaVm);
            }

            try
            {
                var existente = _service.GetById(id);
                if (existente == null)
                    return NotFound();

                var area = _mapper.Map<AreaDeLazer>(areaVm);
                area.CreatedAt = existente.CreatedAt;
                area.ImagemNomeOriginal = existente.ImagemNomeOriginal;
                area.ImagemCaminho = existente.ImagemCaminho;

                if (areaVm.ImagemArquivo is not null && areaVm.ImagemArquivo.Length > 0)
                {
                    _arquivoUploadService.RemoverSeExistir(existente.ImagemCaminho);
                    var arquivo = await _arquivoUploadService.SalvarAsync(areaVm.ImagemArquivo, "areas-lazer");
                    area.ImagemNomeOriginal = arquivo.arquivoNomeOriginal;
                    area.ImagemCaminho = arquivo.arquivoCaminho;
                }

                _service.Edit(area);
                TempData["Sucesso"] = "Area de lazer atualizada com sucesso.";
                RegistrarNotificacao(
                    area.CondominioId,
                    "Area de lazer atualizada",
                    $"A area \"{area.Nome}\" foi atualizada.",
                    Url.Action(nameof(Details), new { id = area.Id }) ?? Url.Action(nameof(Index)) ?? "/AreaDeLazer");

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel atualizar a area de lazer agora.";
            }

            PopularDropdowns(areaVm.CondominioId, areaVm.SindicoId);
            return View(areaVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            return View(_mapper.Map<AreaDeLazerViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = _service.GetById(id);
                if (item != null)
                {
                    _arquivoUploadService.RemoverSeExistir(item.ImagemCaminho);
                    _service.Delete(id);
                    TempData["Sucesso"] = "Area de lazer removida com sucesso.";
                    RegistrarNotificacao(
                        item.CondominioId,
                        "Area de lazer removida",
                        $"A area \"{item.Nome}\" foi removida.",
                        Url.Action(nameof(Index)) ?? "/AreaDeLazer");
                }
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel remover a area de lazer agora.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns(int? condominioSelecionado = null, int? sindicoSelecionado = null)
        {
            condominioSelecionado ??= _condominioContextService.GetCondominioAtualId();

            ViewBag.Condominios = new SelectList(
                _condominioService.GetAll().OrderBy(c => c.Nome).ToList(),
                "Id",
                "Nome",
                condominioSelecionado);
            ViewBag.Sindicos = new SelectList(
                _sindicoService.GetAll()
                    .OrderBy(s => s.Nome)
                    .ToList(),
                "Id",
                "Nome",
                sindicoSelecionado);
        }

        private void RegistrarNotificacao(int condominioId, string titulo, string mensagem, string? urlDestino)
        {
            _notificacaoService.Criar(
                User.Identity?.Name ?? "sistema",
                User.Identity?.Name ?? "Sistema",
                titulo,
                mensagem,
                "info",
                condominioId,
                urlDestino);
        }
    }
}
