using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class VisitanteController : Controller
    {
        private readonly IVisitanteService _service;
        private readonly IMapper _mapper;

        public VisitanteController(IVisitanteService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var lista = _service.GetAll();
            var listaVm = _mapper.Map<List<VisitanteViewModel>>(lista);
            return View(listaVm);
        }

        public ActionResult Detail(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<VisitanteViewModel>(entity));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VisitanteViewModel visitanteVm)
        {
            if (ModelState.IsValid)
            {
                var visitante = _mapper.Map<Visitantes>(visitanteVm);
                _service.Create(visitante);
                return RedirectToAction(nameof(Index));
            }
            return View(visitanteVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<VisitanteViewModel>(item);
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VisitanteViewModel visitanteVm)
        {
            if (id != visitanteVm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var visitante = _mapper.Map<Visitantes>(visitanteVm);
                _service.Edit(visitante);
                return RedirectToAction(nameof(Index));
            }
            return View(visitanteVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<VisitanteViewModel>(item);
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