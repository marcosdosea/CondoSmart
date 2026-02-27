using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class CondominioController : Controller
    {
        private readonly ICondominioService _service;
        private readonly IMapper _mapper;

        public CondominioController(ICondominioService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<CondominioViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CondominioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Condominio>(vm);
            _service.Create(entity);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CondominioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _service.Edit(_mapper.Map<Condominio>(vm));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetEndereco(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var endereco = new
            {
                rua = entity.Rua,
                numero = entity.Numero,
                bairro = entity.Bairro,
                complemento = entity.Complemento,
                cidade = entity.Cidade,
                uf = entity.Uf,
                cep = entity.Cep
            };

            return Json(endereco);
        }
    }
}
