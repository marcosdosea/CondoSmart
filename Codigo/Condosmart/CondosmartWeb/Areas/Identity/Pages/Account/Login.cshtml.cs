using Core.DTO;
using Core.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public LoginModel(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ReturnUrl { get; set; } = string.Empty;

        public class InputModel
        {
            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Senha { get; set; } = string.Empty;

            [Display(Name = "Lembrar-me")]
            public bool LembrarMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            await HttpContext.SignOutAsync();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            var resultado = await _autenticacaoService.LoginAsync(new LoginDTO
            {
                Email = Input.Email,
                Senha = Input.Senha,
                LembrarMe = Input.LembrarMe
            });

            if (resultado.Succeeded)
                return LocalRedirect(ReturnUrl);

            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return Page();
        }
    }
}
