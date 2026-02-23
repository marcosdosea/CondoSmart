using AutoMapper;
using CondosmartWeb.Models;
using Core.Data; // Necessário para acessar o banco e preencher as listas
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Necessário para SelectList

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = "Admin,Sindico,Morador")]
    public class ReservaController : Controller
    {
        private readonly IReservaService _service;
        private readonly CondosmartContext _context; // Adicionado para carregar Dropdowns
        private readonly IMapper _mapper;

        // Injetamos o Contexto aqui no construtor
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
            CarregarListas(); // Preenche os dropdowns antes de abrir a tela
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaViewModel reservaVm)
        {
            // Tapa-buraco: Define CondominioId = 1 se vier 0 (já que não temos login ainda)
            if (reservaVm.CondominioId == 0) reservaVm.CondominioId = 1;

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
                    // Captura erros de validação do Service (ex: datas inválidas) e joga na tela
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Se algo deu errado, recarrega os dropdowns para a tela não quebrar
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
            if (reservaVm.CondominioId == 0) reservaVm.CondominioId = 1;

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

        // Método auxiliar para não repetir código
        private void CarregarListas()
        {
            // Carrega Areas e Moradores do banco para o Select da tela
            ViewBag.AreaId = new SelectList(_context.AreaDeLazer, "Id", "Nome");
            ViewBag.MoradorId = new SelectList(_context.Moradores, "Id", "Nome");

            // Lista estática de status
            ViewBag.Status = new SelectList(new[] { "pendente", "confirmado", "cancelado", "concluido" });
        }
    }
}