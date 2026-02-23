using AutoMapper;
using CondosmartWeb.Models;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Exceptions;

namespace CondosmartWeb.Controllers
{
    public class ReservaController : Controller
    {
        private readonly IReservaService _service;
        private readonly CondosmartContext _context;
        private readonly IMapper _mapper;

        public ReservaController(IReservaService service, CondosmartContext context, IMapper mapper)
        {
            _service = service;
            _context = context;
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
            CarregarListas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaViewModel reservaVm)
        {
            if (reservaVm.CondominioId == 0) reservaVm.CondominioId = 1;

            if (ModelState.IsValid)
            {
                try
                {
                    var reserva = _mapper.Map<Reserva>(reservaVm);
                    _service.Create(reserva);
                    return RedirectToAction(nameof(Index));
                }
                catch (ServiceException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            CarregarListas();
            return View(reservaVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();

            var itemVm = _mapper.Map<ReservaViewModel>(item);
            CarregarListas();
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ReservaViewModel reservaVm)
        {
            if (id != reservaVm.Id) return NotFound();
            if (reservaVm.CondominioId == 0) reservaVm.CondominioId = 1;

            if (ModelState.IsValid)
            {
                try
                {
                    var reserva = _mapper.Map<Reserva>(reservaVm);
                    _service.Edit(reserva);
                    return RedirectToAction(nameof(Index));
                }
                catch (ServiceException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            CarregarListas();
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

        private void CarregarListas()
        {
            ViewBag.AreaId = new SelectList(_context.AreaDeLazer, "Id", "Nome");
            ViewBag.MoradorId = new SelectList(_context.Moradores, "Id", "Nome");
            ViewBag.Status = new SelectList(new[] { "pendente", "confirmado", "cancelado", "concluido" });
        }
    }
}
