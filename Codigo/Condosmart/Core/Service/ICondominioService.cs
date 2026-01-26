using Core.Models;

namespace Core.Service
{
    public interface ICondominioService
    {
        int Create(Condominio condominio);
        void Edit(Condominio condominio);
        void Delete(int id);
        Condominio GetById(int id);
        List<Condominio> GetAll();
    }
}
