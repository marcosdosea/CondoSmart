using Core.Models;
using Core.Data; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Service
{
    public class ChamadosService : IChamadosService
    {
        private readonly CondosmartContext _context;

        public ChamadosService(CondosmartContext context)
        {
            _context = context;
        }

        public int Create(Chamado chamado)
        {
            _context.Chamados.Add(chamado);
            _context.SaveChanges();
            return chamado.Id;
        }

        public void Edit(Chamado chamado)
        {
            _context.Chamados.Update(chamado);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var chamado = _context.Chamados.Find(id);
            if (chamado != null)
            {
                _context.Chamados.Remove(chamado);
                _context.SaveChanges();
            }
        }

        public Chamado? GetById(int id)
        {
            return _context.Chamados.Find(id);
        }

        public List<Chamado> GetAll()
        {
            return _context.Chamados.ToList();
        }

        public void RegistrarChamadoMorador(Chamado chamado)
        {
            if (string.IsNullOrWhiteSpace(chamado.Descricao))
                throw new ArgumentException("A descrição do chamado não pode ser vazia.");

            chamado.Status = "aberto";
            chamado.DataChamado = DateTime.Now;
            chamado.CreatedAt = DateTime.Now;

            _context.Chamados.Add(chamado);
            _context.SaveChanges();
        }
    }
}