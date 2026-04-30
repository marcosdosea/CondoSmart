using Core.Models;

namespace Core.Service
{
    public interface IUnidadesResidenciaisService
    {
        int Create(UnidadesResidenciais unidadesResidenciais);
        void Edit(UnidadesResidenciais unidadesResidenciais);
        void Delete(int id);
        UnidadesResidenciais? GetById(int id);
        UnidadesResidenciais? GetByMoradorId(int moradorId);
        List<UnidadesResidenciais> GetAll();
    }
}
