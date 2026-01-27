using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class UnidadesResidenciaisController : Controller
    {
        private readonly IUnidadesResidenciaisService _service;
        private readonly IMapper _mapper;

        public UnidadesResidenciaisController(
            IUnidadesResidenciaisService service,
            IMapper mapper)
        {
            _service = service;
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var entity = _mapper.Map<UnidadesResidenciais>(vm);
            _service.Create(entity);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var entity = _mapper.Map<UnidadesResidenciais>(vm);
            _service.Edit(entity);

            return RedirectToAction(nameof(Index));
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
    }
}
