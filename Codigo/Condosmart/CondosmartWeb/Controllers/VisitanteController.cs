using AutoMapper;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class VisitanteController : Controller
    {
        private readonly IVisitanteService _visitanteService;
        private readonly IMoradorService _moradorService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public VisitanteController(
            IVisitanteService visitanteService,
            IMoradorService moradorService,
            ICondominioContextService condominioContextService,
            INotificacaoService notificacaoService,
            IMapper mapper)
        {
            _visitanteService = visitanteService;
            _moradorService = moradorService;
            _condominioContextService = condominioContextService;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        // GET: Visitante
        public ActionResult Index()
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            var visitantes = _visitanteService.GetAll()
                .Where(v => !condominioAtualId.HasValue || (v.Morador != null && v.Morador.CondominioId == condominioAtualId.Value))
                .ToList();
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
                MoradoresDisponiveis = ListarMoradoresDoContexto()
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
                TempData["Sucesso"] = "Visitante cadastrado com sucesso.";
                RegistrarNotificacao(visitanteViewModel.MoradorId, "Visitante cadastrado", $"O visitante {visitanteViewModel.Nome} foi cadastrado.");
                return RedirectToAction(nameof(Index));
            }
            
            visitanteViewModel.MoradoresDisponiveis = ListarMoradoresDoContexto();
            return View(visitanteViewModel);
        }

        // GET: Visitante/Edit/5
        public ActionResult Edit(int id)
        {
            var visitante = _visitanteService.GetById(id);
            var visitanteVM = _mapper.Map<VisitanteViewModel>(visitante);
            visitanteVM.MoradoresDisponiveis = ListarMoradoresDoContexto();
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
                TempData["Sucesso"] = "Visitante atualizado com sucesso.";
                RegistrarNotificacao(visitanteViewModel.MoradorId, "Visitante atualizado", $"O visitante {visitanteViewModel.Nome} foi atualizado.");
                return RedirectToAction(nameof(Index));
            }
            
            visitanteViewModel.MoradoresDisponiveis = ListarMoradoresDoContexto();
            return View(visitanteViewModel);
        }

        // GET: Visitante/Delete/5
        public ActionResult Delete(int id)
        {
            var visitante = _visitanteService.GetById(id);
            var visitanteVM = _mapper.Map<VisitanteViewModel>(visitante);
            return View(visitanteVM);
        }

        // POST: Visitante/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var visitante = _visitanteService.GetById(id);
                _visitanteService.Delete(id);
                TempData["Sucesso"] = "Visitante removido com sucesso.";
                if (visitante != null)
                    RegistrarNotificacao(visitante.MoradorId, "Visitante removido", $"O visitante {visitante.Nome} foi removido.");
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel remover o visitante agora.";
            }

            return RedirectToAction(nameof(Index));
        }

        private List<Morador> ListarMoradoresDoContexto()
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            return _moradorService.GetAll()
                .Where(m => !condominioAtualId.HasValue || m.CondominioId == condominioAtualId.Value)
                .OrderBy(m => m.Nome)
                .ToList();
        }

        private void RegistrarNotificacao(int? moradorId, string titulo, string mensagem)
        {
            var condominioId = _condominioContextService.GetCondominioAtualId();
            if (moradorId.HasValue)
                condominioId = _moradorService.GetById(moradorId.Value)?.CondominioId ?? condominioId;

            _notificacaoService.Criar(
                User.Identity?.Name ?? "sistema",
                User.Identity?.Name ?? "Sistema",
                titulo,
                mensagem,
                "info",
                condominioId,
                Url.Action(nameof(Index)) ?? "/Visitante");
        }
    }
}
