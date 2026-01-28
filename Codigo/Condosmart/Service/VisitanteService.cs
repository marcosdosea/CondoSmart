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
            ValidarVisitante(visitante);
            _context.Add(visitante);
            _context.SaveChanges();
            return visitante.Id;
        }

        public void Edit(Visitantes visitante)
        {
            ValidarVisitante(visitante);
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

        public Visitantes? Get(int id)
        {
            return _context.Visitantes.Find(id);
        }

        public IEnumerable<Visitantes> GetAll()
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
                throw new ArgumentException("Visitante inválido.");

            if (string.IsNullOrWhiteSpace(visitante.Nome))
                throw new ArgumentException("O nome do visitante é obrigatório.");

            if (!string.IsNullOrWhiteSpace(visitante.Cpf))
            {
                var cpfLimpo = visitante.Cpf.Replace(".", "").Replace("-", "").Trim();
                if (cpfLimpo.Length != 11)
                    throw new ArgumentException("O CPF deve conter 11 caracteres.");
            }

            if (visitante.DataHoraSaida.HasValue && visitante.DataHoraEntrada.HasValue &&
                visitante.DataHoraSaida.Value < visitante.DataHoraEntrada.Value)
            {
                throw new ArgumentException("A data/hora de saída não pode ser menor que a entrada.");
            }
        }
    }
}