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
        private readonly IAreaDeLazerService _areaService;
        private readonly ICondominioService _condominioService;
        private readonly IMapper _mapper;

        public AreaDeLazerController(IAreaDeLazerService areaService, ICondominioService condominioService, IMapper mapper)
        {
            _areaService = areaService;
            _condominioService = condominioService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _areaService.GetAll();
            var vms = _mapper.Map<List<AreaDeLazerViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Create()
        {
            CarregarCondominios();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AreaDeLazerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                CarregarCondominios();
                return View(vm);
            }

            try
            {
                var novaArea = _mapper.Map<AreaDeLazer>(vm);
                _areaService.Create(novaArea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CarregarCondominios();
                return View(vm);
            }
        }

        private void CarregarCondominios()
        {
            var lista = _condominioService.GetAll();
            ViewBag.CondominioId = new SelectList(lista, "Id", "Nome");
        }
    }
}