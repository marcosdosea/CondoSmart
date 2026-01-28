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

        public ActionResult Index()
        {
            var lista = _service.GetAll();
            var listaVm = _mapper.Map<List<ReservaViewModel>>(lista);
            return View(listaVm);
        }

        public ActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<ReservaViewModel>(entity));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaViewModel reservaVm)
        {
            if (ModelState.IsValid)
            {
                var reserva = _mapper.Map<Reserva>(reservaVm);
                _service.Create(reserva);
                return RedirectToAction(nameof(Index));
            }
            return View(reservaVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<ReservaViewModel>(item);
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ReservaViewModel reservaVm)
        {
            if (id != reservaVm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var reserva = _mapper.Map<Reserva>(reservaVm);
                _service.Edit(reserva);
                return RedirectToAction(nameof(Index));
            }
            return View(reservaVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<ReservaViewModel>(item);
            return View(itemVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}