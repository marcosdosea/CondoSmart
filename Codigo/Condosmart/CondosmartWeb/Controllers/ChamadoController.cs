using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CondosmartWeb.Controllers
{
	public class ChamadoController : Controller
	{
		private readonly IChamadosService _service;
		private readonly ICondominioService _condominioService;
		private readonly IMapper _mapper;

		public ChamadoController(IChamadosService service, ICondominioService condominioService, IMapper mapper)
		{
			_service = service;
			_condominioService = condominioService;
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

			try
			{
				// Validar se o condomínio existe
				var condominio = _condominioService.GetById(vm.CondominioId);
				if (condominio == null)
				{
					ModelState.AddModelError("CondominioId", "O condomínio especificado não existe.");
					return View(vm);
				}

				var entity = _mapper.Map<Chamado>(vm);
				_service.Create(entity);
				return RedirectToAction(nameof(Index));
			}
			catch (ArgumentException ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(vm);
			}
			catch (Exception)
			{
				ModelState.AddModelError("", "Erro ao criar chamado. Verifique os dados e tente novamente.");
				return View(vm);
			}
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

			try
			{
				// Validar se o condomínio existe
				var condominio = _condominioService.GetById(vm.CondominioId);
				if (condominio == null)
				{
					ModelState.AddModelError("CondominioId", "O condomínio especificado não existe.");
					return View(vm);
				}

				var entity = _mapper.Map<Chamado>(vm);
				_service.Edit(entity);
				return RedirectToAction(nameof(Index));
			}
			catch (ArgumentException ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(vm);
			}
			catch (Exception)
			{
				ModelState.AddModelError("", "Erro ao editar chamado. Verifique os dados e tente novamente.");
				return View(vm);
			}
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
			try
			{
				_service.Delete(id);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{
				ModelState.AddModelError("", "Erro ao excluir chamado.");
				return RedirectToAction(nameof(Index));
			}
		}
	}
}