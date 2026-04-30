using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class UsuarioSistema : IdentityUser
    {
        public string? NomeCompleto { get; set; }

        public bool SenhaTemporaria { get; set; }
    }
}
