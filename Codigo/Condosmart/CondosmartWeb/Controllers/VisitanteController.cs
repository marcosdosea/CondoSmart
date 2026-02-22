using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = "Admin,Sindico")]
    public class VisitanteController : Controller
    {
        private readonly IVisitanteService _visitanteService;
        private readonly IMoradorService _moradorService;
        private readonly IMapper _mapper;

        public VisitanteController(IVisitanteService visitanteService, IMoradorService moradorService, IMapper mapper)
        {
            _visitanteService = visitanteService;
            _moradorService = moradorService;
            _mapper = mapper;
        }

        // GET: Visitante
        public ActionResult Index()
        {
            var visitantes = _visitanteService.GetAll();
            var listaDeVisitantes = _mapper.Map<List<VisitanteViewModel>>(visitantes);
            return View(listaDeVisitantes);
        }

        // GET: Visitante/Details/5
        public ActionResult Details(int id)
        {
            var visitante = _visitanteService.GetById(id);
            var visitanteVM = _mapper.Map<VisitanteViewModel>(visitante);
            return View(visitanteVM);
        }

        // GET: Visitante/Create
        public ActionResult Create()
        {
            var viewModel = new VisitanteViewModel
            {
                MoradoresDisponiveis = _moradorService.GetAll()
            };
            return View(viewModel);
        }

        // POST: Visitante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VisitanteViewModel visitanteViewModel)
        {
            if (ModelState.IsValid)
            {
                var visitante = _mapper.Map<Visitantes>(visitanteViewModel);
                _visitanteService.Create(visitante);
                return RedirectToAction(nameof(Index));
            }
            
            visitanteViewModel.MoradoresDisponiveis = _moradorService.GetAll();
            return View(visitanteViewModel);
        }

        // GET: Visitante/Edit/5
        public ActionResult Edit(int id)
        {
            var visitante = _visitanteService.GetById(id);
            var visitanteVM = _mapper.Map<VisitanteViewModel>(visitante);
            visitanteVM.MoradoresDisponiveis = _moradorService.GetAll();
            return View(visitanteVM);
        }

        // POST: Visitante/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VisitanteViewModel visitanteViewModel)
        {
            if (id != visitanteViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var visitante = _mapper.Map<Visitantes>(visitanteViewModel);
                _visitanteService.Edit(visitante);
                return RedirectToAction(nameof(Index));
            }
            
            visitanteViewModel.MoradoresDisponiveis = _moradorService.GetAll();
            return View(visitanteViewModel);
        }

        // GET: Visitante/Delete/5
        public ActionResult Delete(int id)
        {
            var visitante = _visitanteService.GetById(id);
            var visitanteVM = _mapper.Map<VisitanteViewModel>(visitante);
            return View(visitanteVM);
        }

        // POST: Visitante/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, VisitanteViewModel collection)
        {
            _visitanteService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}