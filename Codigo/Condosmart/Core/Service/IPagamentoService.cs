using Core.Models;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IPagamentoService
    {
        int Create(Pagamento pagamento);
        void Edit(Pagamento pagamento);
        void Delete(int id);
        Pagamento? Get(int id);
        IEnumerable<Pagamento> GetAll();
    }
}