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
    public class SindicoController : Controller
    {
        private readonly ISindicoService _service;
        private readonly ICondominioContextService _condominioContextService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public SindicoController(
            ISindicoService service,
            ICondominioContextService condominioContextService,
            INotificacaoService notificacaoService,
            IMapper mapper)
        {
            _service = service;
            _condominioContextService = condominioContextService;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<SindicoViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<SindicoViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SindicoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var entity = _mapper.Map<Sindico>(vm);
                _service.Create(entity);
                TempData["Sucesso"] = "Sindico cadastrado com sucesso.";
                RegistrarNotificacao("Sindico cadastrado", $"O sindico {entity.Nome} foi cadastrado.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel cadastrar o sindico agora.";
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<SindicoViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SindicoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var entity = _mapper.Map<Sindico>(vm);
                _service.Edit(entity);
                TempData["Sucesso"] = "Sindico atualizado com sucesso.";
                RegistrarNotificacao("Sindico atualizado", $"O cadastro do sindico {entity.Nome} foi atualizado.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel atualizar o sindico agora.";
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<SindicoViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var entity = _service.GetById(id);
                _service.Delete(id);
                TempData["Sucesso"] = "Sindico removido com sucesso.";
                if (entity != null)
                    RegistrarNotificacao("Sindico removido", $"O sindico {entity.Nome} foi removido.");
            }
            catch
            {
                TempData["Erro"] = "Nao foi possivel remover o sindico agora.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void RegistrarNotificacao(string titulo, string mensagem)
        {
            _notificacaoService.Criar(
                User.Identity?.Name ?? "sistema",
                User.Identity?.Name ?? "Sistema",
                titulo,
                mensagem,
                "info",
                _condominioContextService.GetCondominioAtualId(),
                Url.Action(nameof(Index)) ?? "/Sindico");
        }
    }
}
