using CondosmartWeb.Services;
using Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = Perfis.Admin)]
    public class ContextoCondominioController : Controller
    {
        private readonly ICondominioContextService _condominioContextService;

        public ContextoCondominioController(ICondominioContextService condominioContextService)
        {
            _condominioContextService = condominioContextService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Selecionar(int condominioId, string? returnUrl = null)
        {
            _condominioContextService.SelecionarCondominio(condominioId);
            TempData["Sucesso"] = "Condominio de trabalho atualizado com sucesso.";

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
