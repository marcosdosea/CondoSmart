using Core.Models;

namespace Core.Service
{
    public interface IMoradorService
    {
        int Create(Morador morador);
        void Edit(Morador morador);
        void Delete(int id);
        Morador? GetById(int id);
        Morador? GetByEmail(string email);
        List<Morador> GetAll();
    }
}
