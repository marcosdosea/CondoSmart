using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class AdminBootstrapService : IAdminBootstrapService
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminBootstrapService(UserManager<UsuarioSistema> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CriarAdminAsync(string nomeCompleto, string email, string senha)
        {
            await GarantirPerfisAsync();

            var usuarioExistente = await _userManager.FindByEmailAsync(email);
            if (usuarioExistente is not null)
                return false;

            var usuario = new UsuarioSistema
            {
                NomeCompleto = nomeCompleto,
                UserName = email,
                Email = email,
                SenhaTemporaria = false
            };

            var resultado = await _userManager.CreateAsync(usuario, senha);
            if (!resultado.Succeeded)
                return false;

            var perfil = await _userManager.AddToRoleAsync(usuario, Perfis.Admin);
            return perfil.Succeeded;
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
