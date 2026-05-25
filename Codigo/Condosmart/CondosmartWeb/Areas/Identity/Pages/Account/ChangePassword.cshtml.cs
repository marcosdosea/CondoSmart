using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly SignInManager<UsuarioSistema> _signInManager;

        public ChangePasswordModel(
            UserManager<UsuarioSistema> userManager,
            SignInManager<UsuarioSistema> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Informe sua senha atual.")]
            [DataType(DataType.Password)]
            public string SenhaAtual { get; set; } = string.Empty;

            [Required(ErrorMessage = "Informe a nova senha.")]
            [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string NovaSenha { get; set; } = string.Empty;

            [Required(ErrorMessage = "Confirme a nova senha.")]
            [DataType(DataType.Password)]
            [Compare("NovaSenha", ErrorMessage = "As senhas nao coincidem.")]
            public string ConfirmacaoSenha { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario is null)
                return RedirectToPage("./Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario is null)
                return RedirectToPage("./Login");

            var resultado = await _userManager.ChangePasswordAsync(usuario, Input.SenhaAtual, Input.NovaSenha);
            if (!resultado.Succeeded)
            {
                foreach (var erro in resultado.Errors)
                    ModelState.AddModelError(string.Empty, erro.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(usuario);
            ViewData["Message"] = "Sua senha foi alterada com sucesso!";
            return Page();
        }
    }
}
