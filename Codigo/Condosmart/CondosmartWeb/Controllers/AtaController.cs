using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class AtaController : Controller
    {
        private readonly IAtaService _service;
        private readonly IMapper _mapper;

        public AtaController(IAtaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<AtaViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Detail(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            return View(_mapper.Map<AtaViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AtaViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Ata>(vm);
            _service.Create(entity);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            return View(_mapper.Map<AtaViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AtaViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _service.Edit(_mapper.Map<Ata>(vm));
            return RedirectToAction(nameof(Index));
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
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
