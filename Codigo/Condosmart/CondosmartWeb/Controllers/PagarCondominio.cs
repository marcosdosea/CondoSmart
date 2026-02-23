using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Core.Exceptions;

namespace CondosmartWeb.Controllers
{
    public class PagarCondominioController : Controller
    {
        private readonly IPagamentoService _service;
        private readonly IMapper _mapper;

        public PagarCondominioController(IPagamentoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var lista = _service.GetAll();
            var listaVm = _mapper.Map<List<PagamentoViewModel>>(lista);
            return View(listaVm);
        }

        public ActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<PagamentoViewModel>(entity));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagamentoViewModel pagamentoVm)
        {
            if (ModelState.IsValid)
            {
                var pagamento = _mapper.Map<Pagamento>(pagamentoVm);
                try
                {
                    _service.Create(pagamento);
                    return RedirectToAction(nameof(Index));
                }
                catch (ServiceException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return View(pagamentoVm);
                }
            }
            return View(pagamentoVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<PagamentoViewModel>(item);
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PagamentoViewModel pagamentoVm)
        {
            if (id != pagamentoVm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var pagamento = _mapper.Map<Pagamento>(pagamentoVm);
                try
                {
                    _service.Edit(pagamento);
                    return RedirectToAction(nameof(Index));
                }
                catch (ServiceException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return View(pagamentoVm);
                }
            }
            return View(pagamentoVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            var itemVm = _mapper.Map<PagamentoViewModel>(item);
            return View(itemVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ServiceException e)
            {
                TempData["Erro"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}