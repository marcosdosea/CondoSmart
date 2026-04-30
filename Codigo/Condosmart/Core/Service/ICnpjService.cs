using Core.DTO;

namespace Core.Service
{
    public interface ICnpjService
    {
        bool IsValid(string? cnpj);

        Task<CnpjDataDTO?> ConsultarCnpjAsync(string cnpj);
    }
}
