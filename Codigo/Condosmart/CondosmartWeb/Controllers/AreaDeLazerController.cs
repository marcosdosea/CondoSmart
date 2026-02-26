using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    public class AreaDeLazerController : Controller
    {
        private readonly IAreaDeLazerService _service;
        private readonly ICondominioService _condominioService;
        private readonly ISindicoService _sindicoService;
        private readonly IMapper _mapper;

        public AreaDeLazerController(IAreaDeLazerService service, ICondominioService condominioService, ISindicoService sindicoService, IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _sindicoService = sindicoService;
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
            PopularDropdowns();
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
            PopularDropdowns();
            return View(areaVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            PopularDropdowns();
            return View(_mapper.Map<AreaDeLazerViewModel>(item));
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
            PopularDropdowns();
            return View(areaVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            return View(_mapper.Map<AreaDeLazerViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns()
        {
            ViewBag.Condominios = new SelectList(_condominioService.GetAll(), "Id", "Nome");
            ViewBag.Sindicos = new SelectList(_sindicoService.GetAll(), "Id", "Nome");
        }
    }
}
