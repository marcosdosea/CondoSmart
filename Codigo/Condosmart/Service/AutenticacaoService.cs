using Core.DTO;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly SignInManager<UsuarioSistema> _signInManager;

        public AutenticacaoService(UserManager<UsuarioSistema> userManager, SignInManager<UsuarioSistema> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginDTO loginDTO)
        {
            return await _signInManager.PasswordSignInAsync(
                loginDTO.Email,
                loginDTO.Senha,
                loginDTO.LembrarMe,
                lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegistrarAsync(RegistroDTO registroDTO)
        {
            var usuario = new UsuarioSistema
            {
                NomeCompleto = registroDTO.NomeCompleto,
                UserName = registroDTO.Email,
                Email = registroDTO.Email
            };

            return await _userManager.CreateAsync(usuario, registroDTO.Senha);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
