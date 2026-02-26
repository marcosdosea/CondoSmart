using Core.DTO;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegistroModel : PageModel
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public RegistroModel(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "O nome completo é obrigatório")]
            [Display(Name = "Nome Completo")]
            public string NomeCompleto { get; set; } = string.Empty;

            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
            [Display(Name = "Senha")]
            public string Senha { get; set; } = string.Empty;

            [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
            [DataType(DataType.Password)]
            [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
            [Display(Name = "Confirmar Senha")]
            public string ConfirmacaoSenha { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado = await _autenticacaoService.RegistrarAsync(new RegistroDTO
            {
                NomeCompleto = Input.NomeCompleto,
                Email = Input.Email,
                Senha = Input.Senha,
                ConfirmacaoSenha = Input.ConfirmacaoSenha
            });

            if (resultado.Succeeded)
                return RedirectToPage("./Login");

            foreach (var erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return Page();
        }
    }
}
