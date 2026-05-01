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
    public class CondominioController : Controller
    {
        private readonly ICondominioService _service;
        private readonly ICnpjService _cnpjService;
        private readonly INotificacaoService _notificacaoService;
        private readonly ICondominioContextService _condominioContextService;
        private readonly IMapper _mapper;

        public CondominioController(
            ICondominioService service,
            ICnpjService cnpjService,
            INotificacaoService notificacaoService,
            ICondominioContextService condominioContextService,
            IMapper mapper)
        {
            _service = service;
            _cnpjService = cnpjService;
            _notificacaoService = notificacaoService;
            _condominioContextService = condominioContextService;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var lista = _service.GetAll().OrderBy(c => c.Nome).ToList();
            var vms = _mapper.Map<List<CondominioViewModel>>(lista);
            return View(PagedListViewModel<CondominioViewModel>.Create(vms, page, pageSize));
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CondominioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (!_cnpjService.IsValid(vm.Cnpj))
            {
                ModelState.AddModelError(nameof(vm.Cnpj), "O CNPJ informado e invalido.");
                return View(vm);
            }

            try
            {
                var entidade = _mapper.Map<Condominio>(vm);
                _service.Create(entidade);
                SetSuccess("Condominio cadastrado com sucesso.");
                RegistrarNotificacao(entidade.Id, "Condominio cadastrado", $"O condominio {entidade.Nome} foi cadastrado.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch
            {
                SetError("Nao foi possivel cadastrar o condominio agora.");
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CondominioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (!_cnpjService.IsValid(vm.Cnpj))
            {
                ModelState.AddModelError(nameof(vm.Cnpj), "O CNPJ informado e invalido.");
                return View(vm);
            }

            try
            {
                var entidade = _mapper.Map<Condominio>(vm);
                _service.Edit(entidade);
                SetSuccess("Condominio atualizado com sucesso.");
                RegistrarNotificacao(entidade.Id, "Condominio atualizado", $"O condominio {entidade.Nome} foi atualizado.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch
            {
                SetError("Nao foi possivel atualizar o condominio agora.");
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var entidade = _service.GetById(id);
                _service.Delete(id);
                SetSuccess("Condominio removido com sucesso.");
                if (entidade != null)
                    RegistrarNotificacao(entidade.Id, "Condominio removido", $"O condominio {entidade.Nome} foi removido.");
            }
            catch
            {
                SetError("Nao foi possivel remover o condominio agora.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetEndereco(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

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

        private void SetSuccess(string message)
        {
            if (TempData != null)
                TempData["Sucesso"] = message;
        }

        private void SetError(string message)
        {
            if (TempData != null)
                TempData["Erro"] = message;
        }

        private void RegistrarNotificacao(int condominioId, string titulo, string mensagem)
        {
            _notificacaoService.Criar(
                User.Identity?.Name ?? "sistema",
                User.Identity?.Name ?? "Sistema",
                titulo,
                mensagem,
                "info",
                condominioId > 0 ? condominioId : _condominioContextService.GetCondominioAtualId(),
                Url.Action(nameof(Index)) ?? "/Condominio");
        }
    }
}
