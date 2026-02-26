using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    public class ReservaController : Controller
    {
        private readonly IReservaService _service;
        private readonly ICondominioService _condominioService;
        private readonly IAreaDeLazerService _areaService;
        private readonly IMoradorService _moradorService;
        private readonly IMapper _mapper;

        public ReservaController(IReservaService service, ICondominioService condominioService, IAreaDeLazerService areaService, IMoradorService moradorService, IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _areaService = areaService;
            _moradorService = moradorService;
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
            return View(new ReservaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaViewModel reservaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reserva = _mapper.Map<Reserva>(reservaVm);
                    _service.Create(reserva);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao salvar a reserva: " + ex.Message);
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
            CarregarListas(); // Preenche os dropdowns com os dados atuais selecionados
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ReservaViewModel reservaVm)
        {
            if (id != reservaVm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var reserva = _mapper.Map<Reserva>(reservaVm);
                    _service.Edit(reserva);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao salvar a reserva: " + ex.Message);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void CarregarListas()
        {
            ViewBag.Areas = new SelectList(_areaService.GetAll(), "Id", "Nome");
            ViewBag.Moradores = new SelectList(_moradorService.GetAll(), "Id", "Nome");
            ViewBag.Condominios = new SelectList(_condominioService.GetAll(), "Id", "Nome");
            ViewBag.StatusList = new SelectList(new[] { "pendente", "confirmado", "cancelado", "concluido" });
        }
    }
}