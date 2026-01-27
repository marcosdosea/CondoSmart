using Core.Models;

namespace Core.Service
{
    public interface IAtaService
    {
        int Create(Ata ata);
        void Edit(Ata ata);
        void Delete(int id);
        Ata? GetById(int id);
        List<Ata> GetAll();
    }
}
