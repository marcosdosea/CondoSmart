using Core.DTO;
using Microsoft.AspNetCore.Identity;

namespace Core.Service
{
    public interface IAutenticacaoService
    {
        Task<SignInResult> LoginAsync(LoginDTO loginDTO);
        Task<IdentityResult> RegistrarAsync(RegistroDTO registroDTO);
        Task LogoutAsync();
    }
}
