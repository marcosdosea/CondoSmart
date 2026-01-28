using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
	public class ChamadoController : Controller
	{
		private readonly IChamadosService _service;
		private readonly IMapper _mapper;

		public ChamadoController(IChamadosService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var lista = _service.GetAll();
			var vms = _mapper.Map<List<ChamadoViewModel>>(lista);
			return View(vms);
		}

		public IActionResult Details(int id)
		{
			var entity = _service.GetById(id);
			if (entity == null) return NotFound();
			return View(_mapper.Map<ChamadoViewModel>(entity));
		}

		public IActionResult Create()
		{
			var vm = new ChamadoViewModel { DataChamado = DateTime.Now, Status = "aberto" };
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ChamadoViewModel vm)
		{
			if (!ModelState.IsValid) return View(vm);

			var entity = _mapper.Map<Chamado>(vm);
			_service.Create(entity);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int id)
		{
			var entity = _service.GetById(id);
			if (entity == null) return NotFound();
			return View(_mapper.Map<ChamadoViewModel>(entity));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, ChamadoViewModel vm)
		{
			if (id != vm.Id) return NotFound();

			if (!ModelState.IsValid) return View(vm);

			var entity = _mapper.Map<Chamado>(vm);
			_service.Edit(entity);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var entity = _service.GetById(id);
			if (entity == null) return NotFound();
			return View(_mapper.Map<ChamadoViewModel>(entity));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			_service.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}