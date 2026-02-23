using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class VisitanteService : IVisitanteService
    {
        private readonly CondosmartContext _context;

        public VisitanteService(CondosmartContext context)
        {
            _context = context;
        }

        public int Create(Visitantes visitante)
        {
            // ValidarVisitante(visitante); // Removido: validação feita na ViewModel
            _context.Add(visitante);
            _context.SaveChanges();
            return visitante.Id;
        }

        public void Edit(Visitantes visitante)
        {
            // ValidarVisitante(visitante); // Removido: validação feita na ViewModel
            _context.Update(visitante);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var visitante = _context.Visitantes.Find(id);
            if (visitante != null)
            {
                _context.Remove(visitante);
                _context.SaveChanges();
            }
        }

        public Visitantes? GetById(int id)
        {
            return _context.Visitantes.Find(id);
        }

        public List<Visitantes> GetAll()
        {
            return _context.Visitantes.AsNoTracking().ToList();
        }

        public IEnumerable<Visitantes> GetByNome(string nome)
        {
            return _context.Visitantes
                .AsNoTracking()
                .Where(v => v.Nome.StartsWith(nome))
                .ToList();
        }

        private static void ValidarVisitante(Visitantes visitante)
        {
            if (visitante == null)
                throw new Core.Exceptions.ServiceException("Visitante inválido.");

            if (visitante.DataHoraSaida.HasValue && visitante.DataHoraEntrada.HasValue &&
                visitante.DataHoraSaida.Value < visitante.DataHoraEntrada.Value)
            {
                throw new Core.Exceptions.ServiceException("A data/hora de saída não pode ser menor que a entrada.");
            }
        }
    }
}