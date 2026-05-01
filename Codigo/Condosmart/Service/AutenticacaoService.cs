using Core.DTO;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly SignInManager<UsuarioSistema> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AutenticacaoService(
            UserManager<UsuarioSistema> userManager,
            SignInManager<UsuarioSistema> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<SignInResult> LoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (usuario is not null && await _userManager.CheckPasswordAsync(usuario, loginDTO.Senha))
                await GarantirPerfilPadraoAsync(usuario);

            return await _signInManager.PasswordSignInAsync(
                loginDTO.Email,
                loginDTO.Senha,
                loginDTO.LembrarMe,
                lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegistrarAsync(RegistroDTO registroDTO)
        {
            await GarantirPerfisAsync();

            var usuario = new UsuarioSistema
            {
                NomeCompleto = registroDTO.NomeCompleto,
                UserName = registroDTO.Email,
                Email = registroDTO.Email,
                SenhaTemporaria = false
            };

            var resultado = await _userManager.CreateAsync(usuario, registroDTO.Senha);
            if (!resultado.Succeeded)
                return resultado;

            var perfilResultado = await _userManager.AddToRoleAsync(usuario, Perfis.Morador);
            return perfilResultado.Succeeded ? resultado : perfilResultado;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private async Task GarantirPerfilPadraoAsync(UsuarioSistema usuario)
        {
            await GarantirPerfisAsync();

            var perfisAtuais = await _userManager.GetRolesAsync(usuario);
            if (perfisAtuais.Count == 0)
                await _userManager.AddToRoleAsync(usuario, Perfis.Morador);
        }

        private async Task GarantirPerfisAsync()
        {
            foreach (var perfil in new[] { Perfis.Admin, Perfis.Morador })
            {
                if (!await _roleManager.RoleExistsAsync(perfil))
                    await _roleManager.CreateAsync(new IdentityRole(perfil));
            }
        }
    }
}
