using Core.DTO;
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

        public AreaDeLazerController(IAreaDeLazerService areaService, ICondominioService condominioService)
        {
            _areaService = areaService;
            _condominioService = condominioService;
        }

        public IActionResult Index()
        {
            var lista = _areaService.GetAll();
            return View(lista);
        }

        public IActionResult Create()
        {
            CarregarCondominios();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AreaDeLazerDTO areaDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var novaArea = new AreaDeLazer
                    {
                        Nome = areaDto.Nome,
                        Descricao = areaDto.Descricao,
                        CondominioId = areaDto.CondominioId,
                        Disponibilidade = areaDto.Disponibilidade
                    };

                    _areaService.Create(novaArea);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            CarregarCondominios();
            return View(areaDto);
        }

        private void CarregarCondominios()
        {
            var lista = _condominioService.GetAll();
            ViewBag.CondominioId = new SelectList(lista, "Id", "Nome");
        }
    }
}