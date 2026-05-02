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
    public class UnidadesResidenciaisController : Controller
    {
        private readonly IUnidadesResidenciaisService _service;
        private readonly ICondominioService _condominioService;
        private readonly ICepService _cepService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public UnidadesResidenciaisController(
            IUnidadesResidenciaisService service,
            ICondominioService condominioService,
            ICepService cepService,
            ICondominioContextService condominioContextService,
            INotificacaoService notificacaoService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _cepService = cepService;
            _condominioContextService = condominioContextService;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            var lista = _service.GetAll()
                .Where(u => !condominioAtualId.HasValue || u.CondominioId == condominioAtualId.Value)
                .OrderBy(u => u.Condominio!.Nome)
                .ThenBy(u => u.Identificador)
                .ToList();
            var vms = _mapper.Map<List<UnidadeResidencialViewModel>>(lista);
            return View(PagedListViewModel<UnidadeResidencialViewModel>.Create(vms, page, pageSize));
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        public IActionResult Create()
        {
            var vm = new UnidadeResidencialViewModel
            {
                CondominioId = _condominioContextService.GetCondominioAtualId() ?? 0
            };
            PopularDropdowns(vm.CondominioId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            if (!await _cepService.IsValidAsync(vm.Cep))
            {
                ModelState.AddModelError(nameof(vm.Cep), "O CEP informado e invalido ou nao pode ser consultado agora.");
                PopularDropdowns();
                return View(vm);
            }

            try
            {
                _service.Create(_mapper.Map<UnidadesResidenciais>(vm));
                SetSuccess("Unidade residencial cadastrada com sucesso.");
                RegistrarNotificacao(vm.CondominioId, "Unidade cadastrada", $"A unidade {vm.Identificador} foi cadastrada.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.Identificador), ex.Message);
                PopularDropdowns();
                return View(vm);
            }
            catch
            {
                SetError("Nao foi possivel cadastrar a unidade agora.");
                PopularDropdowns();
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            PopularDropdowns();
            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            if (!await _cepService.IsValidAsync(vm.Cep))
            {
                ModelState.AddModelError(nameof(vm.Cep), "O CEP informado e invalido ou nao pode ser consultado agora.");
                PopularDropdowns();
                return View(vm);
            }

            try
            {
                _service.Edit(_mapper.Map<UnidadesResidenciais>(vm));
                SetSuccess("Unidade residencial atualizada com sucesso.");
                RegistrarNotificacao(vm.CondominioId, "Unidade atualizada", $"A unidade {vm.Identificador} foi atualizada.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.Identificador), ex.Message);
                PopularDropdowns();
                return View(vm);
            }
            catch
            {
                SetError("Nao foi possivel atualizar a unidade agora.");
                PopularDropdowns();
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var entidade = _service.GetById(id);
                _service.Delete(id);
                SetSuccess("Unidade residencial removida com sucesso.");
                if (entidade != null)
                    RegistrarNotificacao(entidade.CondominioId, "Unidade removida", $"A unidade {entidade.Identificador} foi removida.");
            }
            catch
            {
                SetError("Nao foi possivel remover a unidade agora.");
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns(int? condominioSelecionado = null)
        {
            condominioSelecionado ??= _condominioContextService.GetCondominioAtualId();

            var condominios = _condominioService.GetAll()
                .OrderBy(c => c.Nome)
                .ToList();
            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome", condominioSelecionado);

            var dados = condominios.Select(c => new
            {
                id = c.Id,
                rua = c.Rua,
                numero = c.Numero,
                bairro = c.Bairro,
                complemento = c.Complemento,
                cidade = c.Cidade,
                uf = c.Uf,
                cep = c.Cep
            }).ToList();

            ViewBag.CondominiosData = System.Text.Json.JsonSerializer.Serialize(dados);
        }

        private void SetSuccess(string message)
        {
            if (TempData != null)
                TempData["Sucesso"] = message;
        }

        private void SetError(string message)
        {
            if (TempData != null)
                TempData["Erro"] = message;
        }

        private void RegistrarNotificacao(int condominioId, string titulo, string mensagem)
        {
            if (condominioId <= 0)
                return;

            _notificacaoService.Criar(
                User.Identity?.Name ?? "sistema",
                User.Identity?.Name ?? "Sistema",
                titulo,
                mensagem,
                "info",
                condominioId,
                Url.Action(nameof(Index)) ?? "/UnidadesResidenciais");
        }
    }
}
