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
    public class MoradorController : Controller
    {
        private readonly IMoradorService _service;
        private readonly ICondominioService _condominioService;
        private readonly IUnidadesResidenciaisService _unidadesService;
        private readonly IMoradorProvisionamentoService _provisionamentoService;
        private readonly IMapper _mapper;

        public MoradorController(
            IMoradorService service,
            ICondominioService condominioService,
            IUnidadesResidenciaisService unidadesService,
            IMoradorProvisionamentoService provisionamentoService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _unidadesService = unidadesService;
            _provisionamentoService = provisionamentoService;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var lista = _service.GetAll().OrderBy(m => m.Nome).ToList();
            var vms = _mapper.Map<List<MoradorViewModel>>(lista);
            return View(PagedListViewModel<MoradorViewModel>.Create(vms, page, pageSize));
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<MoradorViewModel>(entity);
            vm.UnidadeId = _unidadesService.GetByMoradorId(entity.Id)?.Id
                ?? _unidadesService.GetAll().FirstOrDefault(u => u.MoradorId == entity.Id)?.Id;

            return View(vm);
        }

        public IActionResult Create()
        {
            PopularDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoradorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }

            try
            {
                var entity = _mapper.Map<Morador>(vm);
                var loginUrl = "/Identity/Account/Login";

                try
                {
                    loginUrl = Url?.Page("/Account/Login", pageHandler: null, values: null, protocol: HttpContext?.Request?.Scheme ?? "https")
                        ?? loginUrl;
                }
                catch
                {
                    // Fallback usado principalmente em cenarios de teste sem contexto HTTP completo.
                }

                var resultado = await _provisionamentoService.CadastrarComAcessoAsync(entity, vm.UnidadeId!.Value, loginUrl);

                SetTempData("Sucesso", $"Morador {resultado.NomeMorador} cadastrado com acesso liberado.");
                SetTempData("ProvisioningMessage", $"Morador {resultado.NomeMorador} cadastrado com acesso liberado.");
                SetTempData("ProvisioningEmail", resultado.Email);
                SetTempData("ProvisioningPassword", resultado.SenhaTemporaria);
                SetTempData("ProvisioningLink", resultado.UrlAcesso);
                SetTempData("ProvisioningStatus", resultado.EmailEnviado ? "enviado" : "pendente");
                SetTempData("ProvisioningWarning", resultado.ObservacaoEmail);

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }
            catch
            {
                SetTempData("Erro", "Nao foi possivel cadastrar o morador agora.");
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<MoradorViewModel>(entity);
            vm.UnidadeId = _unidadesService.GetByMoradorId(entity.Id)?.Id
                ?? _unidadesService.GetAll().FirstOrDefault(u => u.MoradorId == entity.Id)?.Id;

            PopularDropdowns(vm.CondominioId, vm.UnidadeId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MoradorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }

            try
            {
                var anterior = _service.GetById(vm.Id);
                var entity = _mapper.Map<Morador>(vm);

                _service.Edit(entity);
                await _provisionamentoService.AtualizarVinculoUnidadeAsync(vm.Id, vm.UnidadeId!.Value, vm.CondominioId!.Value);
                await _provisionamentoService.AtualizarContaMoradorAsync(anterior?.Email, entity);

                SetTempData("Sucesso", "Morador atualizado com sucesso.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }
            catch
            {
                SetTempData("Erro", "Nao foi possivel atualizar o morador agora.");
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<MoradorViewModel>(entity);
            vm.UnidadeId = _unidadesService.GetByMoradorId(entity.Id)?.Id
                ?? _unidadesService.GetAll().FirstOrDefault(u => u.MoradorId == entity.Id)?.Id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var entity = _service.GetById(id);
                if (entity is not null)
                    await _provisionamentoService.RemoverAcessoAsync(id, entity.Email);

                _service.Delete(id);
                SetTempData("Sucesso", "Morador removido com sucesso.");
            }
            catch
            {
                SetTempData("Erro", "Nao foi possivel remover o morador agora.");
            }

            return RedirectToAction(nameof(Index));
        }

        private void SetTempData(string key, string? value)
        {
            if (TempData != null)
                TempData[key] = value;
        }

        private void PopularDropdowns(int? condominioId = null, int? unidadeSelecionada = null)
        {
            var condominios = _condominioService.GetAll()
                .OrderBy(c => c.Nome)
                .ToList();

            var unidades = _unidadesService.GetAll()
                .Where(u =>
                    (!condominioId.HasValue || u.CondominioId == condominioId.Value) &&
                    (!u.MoradorId.HasValue || u.Id == unidadeSelecionada))
                .OrderBy(u => u.Condominio?.Nome)
                .ThenBy(u => u.Identificador)
                .Select(u => new
                {
                    u.Id,
                    Nome = $"{u.Condominio?.Nome} - {u.Identificador}"
                })
                .ToList();

            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome", condominioId);
            ViewBag.Unidades = new SelectList(unidades, "Id", "Nome", unidadeSelecionada);
        }
    }
}
