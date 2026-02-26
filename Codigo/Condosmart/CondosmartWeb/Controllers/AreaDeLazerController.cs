using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = "Admin,Sindico")]
    public class AreaDeLazerController : Controller
    {
        private readonly IAreaDeLazerService _service;
        private readonly IMapper _mapper;

        public AreaDeLazerController(IAreaDeLazerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var lista = _service.GetAll();
            var listaVm = _mapper.Map<List<AreaDeLazerViewModel>>(lista);
            return View(listaVm);
        }

        public ActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<AreaDeLazerViewModel>(entity));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaDeLazerViewModel areaVm)
        {
            if (ModelState.IsValid)
            {
                var area = _mapper.Map<AreaDeLazer>(areaVm);
                _service.Create(area);
                return RedirectToAction(nameof(Index));
            }
            return View(areaVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<AreaDeLazerViewModel>(item);
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AreaDeLazerViewModel areaVm)
        {
            if (id != areaVm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var area = _mapper.Map<AreaDeLazer>(areaVm);
                _service.Edit(area);
                return RedirectToAction(nameof(Index));
            }
            return View(areaVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<AreaDeLazerViewModel>(item);
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
