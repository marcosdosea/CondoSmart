using CondosmartWeb.Services;
using Core.Identity;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin + "," + Perfis.Morador)]
    public class NotificacoesController : Controller
    {
        private readonly INotificacaoService _notificacaoService;
        private readonly ICondominioContextService _condominioContextService;

        public NotificacoesController(INotificacaoService notificacaoService, ICondominioContextService condominioContextService)
        {
            _notificacaoService = notificacaoService;
            _condominioContextService = condominioContextService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remover(int id, string? returnUrl = null)
        {
            _notificacaoService.Remover(id);
            TempData["Sucesso"] = "Notificacao removida.";

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LimparTodas(string? returnUrl = null)
        {
            _notificacaoService.LimparPorCondominio(_condominioContextService.GetCondominioAtualId());
            TempData["Sucesso"] = "Notificacoes removidas com sucesso.";

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
