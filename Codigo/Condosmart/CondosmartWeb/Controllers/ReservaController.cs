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
    public class ReservaController : Controller
    {
        private readonly IReservaService _service;
        private readonly ICondominioService _condominioService;
        private readonly IAreaDeLazerService _areaService;
        private readonly IMoradorService _moradorService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public ReservaController(
            IReservaService service,
            ICondominioService condominioService,
            IAreaDeLazerService areaService,
            IMoradorService moradorService,
            ICondominioContextService condominioContextService,
            INotificacaoService notificacaoService,
            IMapper mapper)
        {
            _service = service;
            _condominioService = condominioService;
            _areaService = areaService;
            _moradorService = moradorService;
            _condominioContextService = condominioContextService;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var condominioAtualId = _condominioContextService.GetCondominioAtualId();
            var lista = _service.GetAll()
                .Where(r => !condominioAtualId.HasValue || r.CondominioId == condominioAtualId.Value)
                .OrderByDescending(r => r.DataInicio)
                .ToList();
            var listaVm = _mapper.Map<List<ReservaViewModel>>(lista);
            return View(listaVm);
        }

        public ActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<ReservaViewModel>(entity));
        }

        public ActionResult Create()
        {
            var vm = new ReservaViewModel
            {
                CondominioId = _condominioContextService.GetCondominioAtualId() ?? 0,
                Status = "pendente",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddHours(1)
            };
            CarregarListas(vm.CondominioId, vm.AreaId, vm.MoradorId);
            return View(vm);
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
                    TempData["Sucesso"] = "Reserva cadastrada com sucesso.";
                    RegistrarNotificacao(reserva.CondominioId, "Reserva cadastrada", $"Uma reserva foi criada para a area #{reserva.AreaId}.");
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    TempData["Erro"] = "Nao foi possivel salvar a reserva agora.";
                    ModelState.AddModelError(string.Empty, "Erro ao salvar a reserva: " + ex.Message);
                }
            }

            CarregarListas(reservaVm.CondominioId, reservaVm.AreaId, reservaVm.MoradorId);
            return View(reservaVm);
        }

        public ActionResult Edit(int id)
        {
            var item = _service.GetById(id);
            if (item == null)
                return NotFound();

            var itemVm = _mapper.Map<ReservaViewModel>(item);
            CarregarListas(itemVm.CondominioId, itemVm.AreaId, itemVm.MoradorId);
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ReservaViewModel reservaVm)
        {
            if (id != reservaVm.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var reserva = _mapper.Map<Reserva>(reservaVm);
                    _service.Edit(reserva);
                    TempData["Sucesso"] = "Reserva atualizada com sucesso.";
                    RegistrarNotificacao(reserva.CondominioId, "Reserva atualizada", $"A reserva #{reserva.Id} foi atualizada para o status {reserva.Status}.");
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    TempData["Erro"] = "Nao foi possivel salvar a reserva agora.";
                    ModelState.AddModelError(string.Empty, "Erro ao salvar a reserva: " + ex.Message);
                }
            }

            CarregarListas(reservaVm.CondominioId, reservaVm.AreaId, reservaVm.MoradorId);
            return View(reservaVm);
        }

        public ActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null)
                return NotFound();
            var itemVm = _mapper.Map<ReservaViewModel>(item);
            return View(itemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = _service.GetById(id);
                _service.Delete(id);
                TempData["Sucesso"] = "Reserva removida com sucesso.";
                if (item != null)
                    RegistrarNotificacao(item.CondominioId, "Reserva removida", $"A reserva #{item.Id} foi removida.");
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel remover a reserva agora.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void CarregarListas(int? condominioSelecionado = null, int? areaSelecionada = null, int? moradorSelecionado = null)
        {
            condominioSelecionado = condominioSelecionado > 0 ? condominioSelecionado : _condominioContextService.GetCondominioAtualId();

            ViewBag.Areas = new SelectList(
                _areaService.GetAll()
                    .Where(a => !condominioSelecionado.HasValue || a.CondominioId == condominioSelecionado.Value)
                    .OrderBy(a => a.Nome)
                    .ToList(),
                "Id",
                "Nome",
                areaSelecionada);

            ViewBag.Moradores = new SelectList(
                _moradorService.GetAll()
                    .Where(m => !condominioSelecionado.HasValue || m.CondominioId == condominioSelecionado.Value)
                    .OrderBy(m => m.Nome)
                    .ToList(),
                "Id",
                "Nome",
                moradorSelecionado);

            ViewBag.Condominios = new SelectList(
                _condominioService.GetAll().OrderBy(c => c.Nome).ToList(),
                "Id",
                "Nome",
                condominioSelecionado);

            ViewBag.StatusList = new SelectList(new[] { "pendente", "confirmado", "cancelado", "concluido" });
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
                Url.Action(nameof(Index)) ?? "/Reserva");
        }
    }
}
