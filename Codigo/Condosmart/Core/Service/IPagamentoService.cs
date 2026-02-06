using Core.Models;

namespace Core.Service
{
    public interface IPagamentoService
    {
        int Create(Pagamento pagamento);
        void Edit(Pagamento pagamento);
        void Delete(int id);
        Pagamento? GetById(int id);
        List<Pagamento> GetAll();
    }
}
