using Core.Models;

namespace Core.Service
{
    public interface IChamadosService
    {
        int Create(Chamado chamado);
        void Edit(Chamado chamado);
        void Delete(int id);
        Chamado GetById(int id);
        List<Chamado> GetAll();
    }
}
