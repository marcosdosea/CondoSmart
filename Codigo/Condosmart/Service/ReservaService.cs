using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>

    /// </summary>
    public class ReservaService : IReservaService
    {
        private readonly CondosmartContext context;

        public ReservaService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova reserva na base de dados
        /// </summary>
        /// <param name="reserva">dados da reserva</param>
        /// <returns>id da reserva</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Reserva reserva)
        {
            ValidarReserva(reserva);

            context.Add(reserva);
            context.SaveChanges();
            return reserva.Id;
        }

        /// <summary>
        /// Editar dados da reserva na base de dados
        /// </summary>
        /// <param name="reserva">dados da reserva</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Reserva reserva)
        {
            ValidarReserva(reserva);

            context.Update(reserva);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover a reserva da base de dados
        /// </summary>
        /// <param name="id">id da reserva</param>
        public void Delete(int id)
        {
            var reserva = context.Reservas.Find(id);
            if (reserva != null)
            {
                context.Remove(reserva);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma reserva na base de dados
        /// </summary>
        /// <param name="id">id da reserva</param>
        /// <returns>dados da reserva</returns>
        public Reserva? GetById(int id)
        {
            return context.Reservas
                .Include(r => r.Area)
                .Include(r => r.Morador)
                .FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Buscar todas as reservas cadastradas
        /// </summary>
        /// <returns>lista de reservas</returns>
        public List<Reserva> GetAll()
        {
            return context.Reservas
                .AsNoTracking()
                .Include(r => r.Area)
                .Include(r => r.Morador)
                .OrderByDescending(r => r.DataInicio)
                .ToList();
        }

        /// <summary>
        /// Valida regras básicas da reserva
        /// </summary>
        /// <param name="reserva"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidarReserva(Reserva reserva)
        {
            if (reserva == null)
                throw new Core.Exceptions.ServiceException("Reserva inválida.");

            if (reserva.DataFim < reserva.DataInicio)
                throw new Core.Exceptions.ServiceException("A data/hora de fim não pode ser anterior à data/hora de início.");
        }
    }
}
