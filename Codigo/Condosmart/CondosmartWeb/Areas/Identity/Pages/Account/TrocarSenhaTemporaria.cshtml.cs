using Core.Identity;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Areas.Identity.Pages.Account
{
    [Authorize]
    public class TrocarSenhaTemporariaModel : PageModel
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly SignInManager<UsuarioSistema> _signInManager;

        public TrocarSenhaTemporariaModel(
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

            if (!usuario.SenhaTemporaria)
                return await RedirecionarDashboardAsync(usuario);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario is null)
                return RedirectToPage("./Login");

            if (!usuario.SenhaTemporaria)
                return await RedirecionarDashboardAsync(usuario);

            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            var resultado = await _userManager.ResetPasswordAsync(usuario, token, Input.NovaSenha);

            if (!resultado.Succeeded)
            {
                foreach (var erro in resultado.Errors)
                    ModelState.AddModelError(string.Empty, erro.Description);

                return Page();
            }

            usuario.SenhaTemporaria = false;
            var updateResult = await _userManager.UpdateAsync(usuario);
            if (!updateResult.Succeeded)
            {
                foreach (var erro in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, erro.Description);

                return Page();
            }

            await _signInManager.RefreshSignInAsync(usuario);
            return await RedirecionarDashboardAsync(usuario);
        }

        private async Task<IActionResult> RedirecionarDashboardAsync(UsuarioSistema usuario)
        {
            if (await _userManager.IsInRoleAsync(usuario, Perfis.Admin))
                return RedirectToAction("Index", "Home");

            if (await _userManager.IsInRoleAsync(usuario, Perfis.Morador))
                return RedirectToAction("Index", "MoradorDashboard");

            return RedirectToPage("./AcessoNegado");
        }
    }
}
