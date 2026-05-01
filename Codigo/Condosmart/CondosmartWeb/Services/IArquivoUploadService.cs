using Microsoft.AspNetCore.Http;

namespace CondosmartWeb.Services
{
    public interface IArquivoUploadService
    {
        Task<(string arquivoNomeOriginal, string arquivoCaminho)> SalvarAsync(IFormFile arquivo, string subpasta);
        void RemoverSeExistir(string? arquivoCaminho);
    }
}
