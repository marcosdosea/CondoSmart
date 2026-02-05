using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly IMapper _mapper;

        public PagamentoController(IPagamentoService pagamentoService, IMapper mapper)
        {
            _pagamentoService = pagamentoService;
            _mapper = mapper;
        }

        // GET: Pagamento
        public ActionResult Index()
        {
            var listaPagamentos = _pagamentoService.GetAll();
            var listaPagamentosModel = _mapper.Map<List<PagamentoViewModel>>(listaPagamentos);
            return View(listaPagamentosModel);
        }

        // GET: Pagamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pagamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagamentoViewModel pagamentoModel)
        {
            if (ModelState.IsValid)
            {
                var pagamento = _mapper.Map<Pagamento>(pagamentoModel);
                _pagamentoService.Create(pagamento);
                return RedirectToAction(nameof(Index));
            }
            return View(pagamentoModel);
        }

        // GET: Pagamento/Edit/5
        public ActionResult Edit(int id)
        {
            var pagamento = _pagamentoService.Get(id);
            if (pagamento == null) return NotFound();

            var pagamentoModel = _mapper.Map<PagamentoViewModel>(pagamento);
            return View(pagamentoModel);
        }

        // POST: Pagamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PagamentoViewModel pagamentoModel)
        {
            if (id != pagamentoModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var pagamento = _mapper.Map<Pagamento>(pagamentoModel);
                _pagamentoService.Edit(pagamento);
                return RedirectToAction(nameof(Index));
            }
            return View(pagamentoModel);
        }

        // GET: Pagamento/Delete/5
        public ActionResult Delete(int id)
        {
            var pagamento = _pagamentoService.Get(id);
            if (pagamento == null) return NotFound();

            var pagamentoModel = _mapper.Map<PagamentoViewModel>(pagamento);
            return View(pagamentoModel);
        }

        // POST: Pagamento/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            _pagamentoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}