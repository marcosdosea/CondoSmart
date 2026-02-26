using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CondosmartWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class AcessoNegadoModel : PageModel
    {
        public void OnGet() { }
    }
}
