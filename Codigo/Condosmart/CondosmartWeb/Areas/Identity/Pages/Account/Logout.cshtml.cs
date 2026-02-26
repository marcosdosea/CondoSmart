using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CondosmartWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public LogoutModel(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _autenticacaoService.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
