using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    public class AtaController : Controller
    {
        private readonly IAtaService _service;
        private readonly ICondominioService _condominioService;
        private readonly ISindicoService _sindicoService;
        private readonly IMapper _mapper;

        public AtaController(IAtaService service, ICondominioService condominioService, ISindicoService sindicoService, IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _sindicoService = sindicoService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<AtaViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            return View(_mapper.Map<AtaViewModel>(entity));
        }

        public IActionResult Create()
        {
            PopularDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AtaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            var entity = _mapper.Map<Ata>(vm);
            _service.Create(entity);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            PopularDropdowns();
            return View(_mapper.Map<AtaViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AtaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

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

        private void PopularDropdowns()
        {
            var condominios = _condominioService.GetAll();
            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome");

            var sindicos = _sindicoService.GetAll();
            ViewBag.Sindicos = new SelectList(sindicos, "Id", "Nome");
        }
    }
}
