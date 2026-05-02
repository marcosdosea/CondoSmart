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
    public class AtaController : Controller
    {
        private readonly IAtaService _service;
        private readonly ICondominioService _condominioService;
        private readonly ISindicoService _sindicoService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly IArquivoUploadService _arquivoUploadService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public AtaController(
            IAtaService service,
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

        public IActionResult Index()
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            var lista = _service.GetAll()
                .Where(a => !condominioAtualId.HasValue || a.CondominioId == condominioAtualId.Value)
                .OrderByDescending(a => a.DataReuniao)
                .ToList();
            var vms = _mapper.Map<List<AtaViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            return View(_mapper.Map<AtaViewModel>(entity));
        }

        public IActionResult Create()
        {
            var vm = new AtaViewModel
            {
                DataReuniao = DateTime.Today,
                CondominioId = _condominioContextService.GetCondominioAtualId() ?? 0
            };

            PopularDropdowns(vm.CondominioId, vm.SindicoId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AtaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns(vm.CondominioId, vm.SindicoId);
                return View(vm);
            }

            try
            {
                var entity = _mapper.Map<Ata>(vm);
                entity.ArquivoNomeOriginal = vm.ArquivoNomeOriginal;
                entity.ArquivoCaminho = vm.ArquivoCaminho;

                if (vm.ArquivoAta is not null && vm.ArquivoAta.Length > 0)
                {
                    var arquivo = await _arquivoUploadService.SalvarAsync(vm.ArquivoAta, "atas");
                    entity.ArquivoNomeOriginal = arquivo.arquivoNomeOriginal;
                    entity.ArquivoCaminho = arquivo.arquivoCaminho;
                }

                var id = _service.Create(entity);
                TempData["Sucesso"] = "Ata cadastrada com sucesso.";
                RegistrarNotificacao(
                    entity.CondominioId,
                    "Ata cadastrada",
                    $"A ata \"{entity.Titulo}\" foi cadastrada.",
                    Url.Action(nameof(Details), new { id }) ?? Url.Action(nameof(Index)) ?? "/Ata");

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel cadastrar a ata agora.";
            }

            PopularDropdowns(vm.CondominioId, vm.SindicoId);
            return View(vm);
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<AtaViewModel>(entity);
            PopularDropdowns(vm.CondominioId, vm.SindicoId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AtaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns(vm.CondominioId, vm.SindicoId);
                return View(vm);
            }

            try
            {
                var existente = _service.GetById(vm.Id);
                if (existente == null)
                    return NotFound();

                var entity = _mapper.Map<Ata>(vm);
                entity.CreatedAt = existente.CreatedAt;
                entity.ArquivoNomeOriginal = existente.ArquivoNomeOriginal;
                entity.ArquivoCaminho = existente.ArquivoCaminho;

                if (vm.ArquivoAta is not null && vm.ArquivoAta.Length > 0)
                {
                    _arquivoUploadService.RemoverSeExistir(existente.ArquivoCaminho);
                    var arquivo = await _arquivoUploadService.SalvarAsync(vm.ArquivoAta, "atas");
                    entity.ArquivoNomeOriginal = arquivo.arquivoNomeOriginal;
                    entity.ArquivoCaminho = arquivo.arquivoCaminho;
                }

                _service.Edit(entity);
                TempData["Sucesso"] = "Ata atualizada com sucesso.";
                RegistrarNotificacao(
                    entity.CondominioId,
                    "Ata atualizada",
                    $"A ata \"{entity.Titulo}\" foi atualizada.",
                    Url.Action(nameof(Details), new { id = entity.Id }) ?? Url.Action(nameof(Index)) ?? "/Ata");

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel atualizar a ata agora.";
            }

            PopularDropdowns(vm.CondominioId, vm.SindicoId);
            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            return View(_mapper.Map<AtaViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var entity = _service.GetById(id);
                if (entity != null)
                {
                    _arquivoUploadService.RemoverSeExistir(entity.ArquivoCaminho);
                    _service.Delete(id);
                    TempData["Sucesso"] = "Ata removida com sucesso.";
                    RegistrarNotificacao(
                        entity.CondominioId,
                        "Ata removida",
                        $"A ata \"{entity.Titulo}\" foi removida.",
                        Url.Action(nameof(Index)) ?? "/Ata");
                }
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel remover a ata agora.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns(int? condominioSelecionado = null, int? sindicoSelecionado = null)
        {
            condominioSelecionado ??= _condominioContextService.GetCondominioAtualId();

            var condominios = _condominioService.GetAll()
                .OrderBy(c => c.Nome)
                .ToList();
            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome", condominioSelecionado);

            var sindicos = _sindicoService.GetAll()
                .OrderBy(s => s.Nome)
                .ToList();
            ViewBag.Sindicos = new SelectList(sindicos, "Id", "Nome", sindicoSelecionado);
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
