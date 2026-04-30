using Core.DTO;
using Core.Models;

namespace Core.Service
{
    public interface IMoradorProvisionamentoService
    {
        Task<ProvisionamentoMoradorResultDTO> CadastrarComAcessoAsync(Morador morador, int unidadeId, string urlAcesso);
        Task AtualizarVinculoUnidadeAsync(int moradorId, int unidadeId, int condominioId);
        Task AtualizarContaMoradorAsync(string? emailAnterior, Morador morador);
        Task RemoverAcessoAsync(int moradorId, string? email);
    }
}
