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
    }
}
