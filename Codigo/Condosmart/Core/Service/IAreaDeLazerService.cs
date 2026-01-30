using Core.Models;

namespace Core.Service
{
    public interface IAreaDeLazerService
    {
        int Create(AreaDeLazer areaDeLazer);
        void Edit(AreaDeLazer areaDeLazer);
        void Delete(int id);
        AreaDeLazer? GetById(int id);
        List<AreaDeLazer> GetAll();
    }
}
