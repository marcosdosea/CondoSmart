using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    public class UnidadesResidenciaisController : Controller
    {
        private readonly IUnidadesResidenciaisService _service;
        private readonly ICondominioService _condominioService;
        private readonly IMapper _mapper;

        public UnidadesResidenciaisController(
            IUnidadesResidenciaisService service,
            ICondominioService condominioService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<UnidadeResidencialViewModel>>(lista);
            return View(vms);
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
        public IActionResult Create(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            try
            {
                var entity = _mapper.Map<UnidadesResidenciais>(vm);
                _service.Create(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.Identificador), ex.Message);
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
        public IActionResult Edit(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            try
            {
                var entity = _mapper.Map<UnidadesResidenciais>(vm);
                _service.Edit(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.Identificador), ex.Message);
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
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns()
        {
            var condominios = _condominioService.GetAll();
            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome");
        }
    }
}

