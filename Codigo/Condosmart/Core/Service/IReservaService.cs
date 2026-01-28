using Core.Models;

namespace Core.Service
{
    public interface IReservaService
    {
        int Create(Reserva reserva);
        void Edit(Reserva reserva);
        void Delete(int id);
        Reserva? GetById(int id);
        List<Reserva> GetAll();
    }
}
