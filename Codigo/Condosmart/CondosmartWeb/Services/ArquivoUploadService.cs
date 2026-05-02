using Microsoft.AspNetCore.Http;

namespace CondosmartWeb.Services
{
    public class ArquivoUploadService : IArquivoUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public ArquivoUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<(string arquivoNomeOriginal, string arquivoCaminho)> SalvarAsync(IFormFile arquivo, string subpasta)
        {
            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeGerado = $"{Guid.NewGuid():N}{extensao}";
            var pastaFisica = Path.Combine(_environment.WebRootPath, "uploads", subpasta);
            Directory.CreateDirectory(pastaFisica);

            var caminhoFisico = Path.Combine(pastaFisica, nomeGerado);
            await using var stream = new FileStream(caminhoFisico, FileMode.Create);
            await arquivo.CopyToAsync(stream);

            return (arquivo.FileName, $"/uploads/{subpasta}/{nomeGerado}");
        }

        public void RemoverSeExistir(string? arquivoCaminho)
        {
            if (string.IsNullOrWhiteSpace(arquivoCaminho))
                return;

            var caminhoNormalizado = arquivoCaminho.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var caminhoFisico = Path.Combine(_environment.WebRootPath, caminhoNormalizado);

            if (File.Exists(caminhoFisico))
                File.Delete(caminhoFisico);
        }
    }
}
