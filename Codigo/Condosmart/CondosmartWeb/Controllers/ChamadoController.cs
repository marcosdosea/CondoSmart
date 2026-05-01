using AutoMapper;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class ChamadoController : Controller
    {
        private readonly IChamadosService _service;
        private readonly ICondominioService _condominioService;
        private readonly IMoradorService _moradorService;
        private readonly ISindicoService _sindicoService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public ChamadoController(
            IChamadosService service,
            ICondominioService condominioService,
            IMoradorService moradorService,
            ISindicoService sindicoService,
            ICondominioContextService condominioContextService,
            INotificacaoService notificacaoService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _moradorService = moradorService;
            _sindicoService = sindicoService;
            _condominioContextService = condominioContextService;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            var lista = _service.GetAll()
                .Where(c => !condominioAtualId.HasValue || c.CondominioId == condominioAtualId.Value)
                .OrderByDescending(c => c.DataChamado)
                .ToList();

            var vms = _mapper.Map<List<ChamadoViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<ChamadoViewModel>(entity));
        }

        public IActionResult Create()
        {
            var vm = new ChamadoViewModel
            {
                DataChamado = DateTime.Now,
                Status = "aberto",
                CondominioId = _condominioContextService.GetCondominioAtualId() ?? 0
            };

            PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChamadoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
                return View(vm);
            }

            try
            {
                var condominio = _condominioService.GetById(vm.CondominioId);
                if (condominio == null)
                {
                    ModelState.AddModelError(nameof(vm.CondominioId), "O condominio especificado nao existe.");
                    PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
                    return View(vm);
                }

                var entity = _mapper.Map<Chamado>(vm);
                _service.Create(entity);

                TempData["Sucesso"] = "Chamado cadastrado com sucesso.";
                RegistrarNotificacao(entity.CondominioId, "Chamado cadastrado", $"Um novo chamado foi aberto com status {entity.Status}.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel criar o chamado agora.";
                ModelState.AddModelError(string.Empty, "Erro ao criar chamado. Verifique os dados e tente novamente.");
            }

            PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
            return View(vm);
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            var vm = _mapper.Map<ChamadoViewModel>(entity);
            PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ChamadoViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
                return View(vm);
            }

            try
            {
                var condominio = _condominioService.GetById(vm.CondominioId);
                if (condominio == null)
                {
                    ModelState.AddModelError(nameof(vm.CondominioId), "O condominio especificado nao existe.");
                    PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
                    return View(vm);
                }

                var entity = _mapper.Map<Chamado>(vm);
                _service.Edit(entity);

                TempData["Sucesso"] = "Chamado atualizado com sucesso.";
                RegistrarNotificacao(entity.CondominioId, "Chamado atualizado", $"O chamado #{entity.Id} foi atualizado para o status {entity.Status}.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel atualizar o chamado agora.";
                ModelState.AddModelError(string.Empty, "Erro ao editar chamado. Verifique os dados e tente novamente.");
            }

            PopularDropdowns(vm.CondominioId, vm.MoradorId, vm.SindicoId);
            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<ChamadoViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var entity = _service.GetById(id);
                _service.Delete(id);
                TempData["Sucesso"] = "Chamado removido com sucesso.";

                if (entity != null)
                    RegistrarNotificacao(entity.CondominioId, "Chamado removido", $"O chamado #{entity.Id} foi removido.");
            }
            catch
            {
                TempData["Erro"] = "Erro ao excluir chamado.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns(int? condominioSelecionado = null, int? moradorSelecionado = null, int? sindicoSelecionado = null)
        {
            condominioSelecionado = condominioSelecionado > 0 ? condominioSelecionado : _condominioContextService.GetCondominioAtualId();

            ViewBag.Condominios = new SelectList(
                _condominioService.GetAll().OrderBy(c => c.Nome).ToList(),
                "Id",
                "Nome",
                condominioSelecionado);

            ViewBag.Moradores = new SelectList(
                _moradorService.GetAll()
                    .Where(m => !condominioSelecionado.HasValue || m.CondominioId == condominioSelecionado.Value)
                    .OrderBy(m => m.Nome)
                    .ToList(),
                "Id",
                "Nome",
                moradorSelecionado);

            ViewBag.Sindicos = new SelectList(
                _sindicoService.GetAll().OrderBy(s => s.Nome).ToList(),
                "Id",
                "Nome",
                sindicoSelecionado);
        }

        private void RegistrarNotificacao(int condominioId, string titulo, string mensagem)
        {
            if (condominioId <= 0)
                return;

            _notificacaoService.Criar(
                User.Identity?.Name ?? "sistema",
                User.Identity?.Name ?? "Sistema",
                titulo,
                mensagem,
                "info",
                condominioId,
                Url.Action(nameof(Index)) ?? "/Chamado");
        }
    }
}
