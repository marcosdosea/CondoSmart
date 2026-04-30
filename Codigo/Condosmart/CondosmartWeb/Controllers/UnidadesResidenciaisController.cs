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
    public class UnidadesResidenciaisController : Controller
    {
        private readonly IUnidadesResidenciaisService _service;
        private readonly ICondominioService _condominioService;
        private readonly ICepService _cepService;
        private readonly IMapper _mapper;

        public UnidadesResidenciaisController(
            IUnidadesResidenciaisService service,
            ICondominioService condominioService,
            ICepService cepService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _cepService = cepService;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var lista = _service.GetAll()
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
            PopularDropdowns();
            return View();
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
                _service.Delete(id);
                SetSuccess("Unidade residencial removida com sucesso.");
            }
            catch
            {
                SetError("Nao foi possivel remover a unidade agora.");
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns()
        {
            var condominios = _condominioService.GetAll();
            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome");

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
    }
}
