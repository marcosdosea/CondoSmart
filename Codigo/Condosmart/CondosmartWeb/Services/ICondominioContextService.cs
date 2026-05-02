using Core.Models;

namespace CondosmartWeb.Services
{
    public interface ICondominioContextService
    {
        int? GetCondominioAtualId();
        Condominio? GetCondominioAtual();
        string GetCondominioAtualNome();
        List<Condominio> GetCondominiosDisponiveis();
        void SelecionarCondominio(int condominioId);
    }
}
