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

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<MoradorViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<MoradorViewModel>(entity);
            vm.UnidadeId = _unidadesService.GetAll()
                .FirstOrDefault(u => u.MoradorId == entity.Id)?.Id;

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
                var loginUrl = Url.Page("/Account/Login", pageHandler: null, values: null, protocol: Request.Scheme) ?? string.Empty;
                var resultado = await _provisionamentoService.CadastrarComAcessoAsync(entity, vm.UnidadeId!.Value, loginUrl);

                TempData["ProvisioningMessage"] = $"Morador {resultado.NomeMorador} cadastrado com acesso liberado.";
                TempData["ProvisioningEmail"] = resultado.Email;
                TempData["ProvisioningPassword"] = resultado.SenhaTemporaria;
                TempData["ProvisioningLink"] = resultado.UrlAcesso;
                TempData["ProvisioningStatus"] = resultado.EmailEnviado ? "enviado" : "pendente";
                TempData["ProvisioningWarning"] = resultado.ObservacaoEmail;

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<MoradorViewModel>(entity);
            vm.UnidadeId = _unidadesService.GetAll()
                .FirstOrDefault(u => u.MoradorId == entity.Id)?.Id;

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

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopularDropdowns(vm.CondominioId, vm.UnidadeId);
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = _mapper.Map<MoradorViewModel>(entity);
            vm.UnidadeId = _unidadesService.GetAll()
                .FirstOrDefault(u => u.MoradorId == entity.Id)?.Id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = _service.GetById(id);
            if (entity is not null)
                await _provisionamentoService.RemoverAcessoAsync(id, entity.Email);

            _service.Delete(id);
            return RedirectToAction(nameof(Index));
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
