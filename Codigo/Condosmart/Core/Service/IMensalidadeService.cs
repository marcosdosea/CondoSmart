using Core.DTO;
using Core.Models;

namespace Core.Service
{
    public interface IMensalidadeService
    {
        int Create(Mensalidade mensalidade);
        void Edit(Mensalidade mensalidade);
        void Delete(int id);
        Mensalidade? GetById(int id);
        List<Mensalidade> GetAll();
        List<Mensalidade> GetByMorador(int moradorId);
        List<Mensalidade> Filtrar(int? condominioId, int? unidadeId, string? status, int? mesCompetencia, int? anoCompetencia);
        List<ConfiguracaoMensalidade> GetConfiguracoes();
        ConfiguracaoMensalidade? GetConfiguracaoAtiva(int condominioId);
        void SalvarConfiguracao(ConfiguracaoMensalidade configuracao);
        GeracaoParcelasResultDTO GerarParcelasEmLote(int condominioId, int anoReferencia, int quantidadeParcelas);
    }
}
