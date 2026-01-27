using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados da reserva
    /// </summary>
    public class ReservaService : IReservaService
    {
        private readonly CondosmartContext context;

        public ReservaService(CondosmartContext context)
        {
            this.context = context;
        }

        public int Create(Reserva reserva)
        {
            ValidarReserva(reserva);

            context.Add(reserva);
            context.SaveChanges();
            return reserva.Id;
        }

        public void Edit(Reserva reserva)
        {
            ValidarReserva(reserva);

            context.Update(reserva);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var reserva = context.Reservas.Find(id);
            if (reserva != null)
            {
                context.Remove(reserva);
                context.SaveChanges();
            }
        }

        public Reserva? GetById(int id)
        {
            return context.Reservas.Find(id);
        }

        public List<Reserva> GetAll()
        {
            return context.Reservas.AsNoTracking().ToList();
        }

        private static void ValidarReserva(Reserva reserva)
        {
            if (reserva == null)
                throw new ArgumentException("Reserva inválida.");

            if (reserva.AreaId <= 0)
                throw new ArgumentException("A área é obrigatória.");

            if (reserva.CondominioId <= 0)
                throw new ArgumentException("O condomínio é obrigatório.");

            if (reserva.DataFim < reserva.DataInicio)
                throw new ArgumentException("A data/hora de fim não pode ser anterior à data/hora de início.");

            if (string.IsNullOrWhiteSpace(reserva.Status))
                throw new ArgumentException("O status da reserva é obrigatório.");

            var allowed = new[] { "confirmado", "pendente", "cancelado", "concluido" };
            if (!allowed.Contains(reserva.Status.ToLowerInvariant()))
                throw new ArgumentException("Status inválido. Valores permitidos: confirmado, pendente, cancelado, concluido.");
        }
    }
}