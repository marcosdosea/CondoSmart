using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class MoradorController : Controller
    {
        private readonly IMoradorService _service;
        private readonly IMapper _mapper;

        public MoradorController(IMoradorService service, IMapper mapper)
        {
            _service = service;
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
            return View(_mapper.Map<MoradorViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MoradorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Morador>(vm);
            _service.Create(entity);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<MoradorViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MoradorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _service.Edit(_mapper.Map<Morador>(vm));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<MoradorViewModel>(entity));
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
