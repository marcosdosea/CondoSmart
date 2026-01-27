using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
            _context.Visitantes.Add(visitante);
            _context.SaveChanges();
            return visitante.Id;
        }

        public void Edit(Visitantes visitante)
        {
            var existing = _context.Visitantes.Find(visitante.Id);
            if (existing == null) throw new KeyNotFoundException("Visitante não encontrado.");

            existing.Nome = visitante.Nome;
            existing.Cpf = visitante.Cpf;
            existing.Telefone = visitante.Telefone;
            existing.Observacao = visitante.Observacao;
            existing.DataHoraEntrada = visitante.DataHoraEntrada;
            existing.DataHoraSaida = visitante.DataHoraSaida;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Visitantes.Find(id);
            if (existing == null) throw new KeyNotFoundException("Visitante não encontrado.");

            _context.Visitantes.Remove(existing);
            _context.SaveChanges();
        }

        public Visitantes GetById(int id)
        {
            var v = _context.Visitantes.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (v == null) throw new KeyNotFoundException("Visitante não encontrado.");
            return v;
        }

        public List<Visitantes> GetAll()
        {
            return _context.Visitantes.AsNoTracking().ToList();
        }
    }
}