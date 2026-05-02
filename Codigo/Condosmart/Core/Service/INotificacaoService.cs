using Core.Models;

namespace Core.Service
{
    public interface INotificacaoService
    {
        void Criar(string usuarioEmail, string usuarioNome, string titulo, string mensagem, string tipo = "info", int? condominioId = null, string? urlDestino = null);
        List<NotificacaoSistema> ListarRecentes(int? condominioId, int take = 15);
        int Contar(int? condominioId);
        void Remover(int id);
        void LimparPorCondominio(int? condominioId);
    }
}
