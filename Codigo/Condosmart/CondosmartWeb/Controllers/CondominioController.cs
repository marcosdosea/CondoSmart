using AutoMapper;
using CondosmartWeb.Models;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class CondominioController : Controller
    {
        private readonly ICondominioService _service;
        private readonly ICnpjService _cnpjService;
        private readonly IMapper _mapper;

        public CondominioController(ICondominioService service, ICnpjService cnpjService, IMapper mapper)
        {
            _service = service;
            _cnpjService = cnpjService;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var lista = _service.GetAll().OrderBy(c => c.Nome).ToList();
            var vms = _mapper.Map<List<CondominioViewModel>>(lista);
            return View(PagedListViewModel<CondominioViewModel>.Create(vms, page, pageSize));
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CondominioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (!_cnpjService.IsValid(vm.Cnpj))
            {
                ModelState.AddModelError(nameof(vm.Cnpj), "O CNPJ informado e invalido.");
                return View(vm);
            }

            try
            {
                _service.Create(_mapper.Map<Condominio>(vm));
                SetSuccess("Condominio cadastrado com sucesso.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch
            {
                SetError("Nao foi possivel cadastrar o condominio agora.");
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CondominioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (!_cnpjService.IsValid(vm.Cnpj))
            {
                ModelState.AddModelError(nameof(vm.Cnpj), "O CNPJ informado e invalido.");
                return View(vm);
            }

            try
            {
                _service.Edit(_mapper.Map<Condominio>(vm));
                SetSuccess("Condominio atualizado com sucesso.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch
            {
                SetError("Nao foi possivel atualizar o condominio agora.");
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.Delete(id);
                SetSuccess("Condominio removido com sucesso.");
            }
            catch
            {
                SetError("Nao foi possivel remover o condominio agora.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetEndereco(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            var endereco = new
            {
                rua = entity.Rua,
                numero = entity.Numero,
                bairro = entity.Bairro,
                complemento = entity.Complemento,
                cidade = entity.Cidade,
                uf = entity.Uf,
                cep = entity.Cep
            };

            return Json(endereco);
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
