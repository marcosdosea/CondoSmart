using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class ReservaController : Controller
    {
        private readonly IReservaService _service;
        private readonly IMapper _mapper;

        public ReservaController(IReservaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<ReservaViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<ReservaViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReservaViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Reserva>(vm);
            _service.Create(entity);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<ReservaViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ReservaViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid) return View(vm);

            _service.Edit(_mapper.Map<Reserva>(vm));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<ReservaViewModel>(entity));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}