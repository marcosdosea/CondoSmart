using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class SindicoService : ISindicoService
    {
        private readonly CondosmartContext _context;

        public SindicoService(CondosmartContext context)
        {
            _context = context;
        }

        public int Create(Sindico sindico)
        {
            _context.Sindicos.Add(sindico);
            _context.SaveChanges();
            return sindico.Id;
        }

        public void Edit(Sindico sindico)
        {
            var existing = _context.Sindicos.Find(sindico.Id);
            if (existing == null) throw new KeyNotFoundException("Síndico não encontrado.");

            existing.Nome = sindico.Nome;
            existing.Cpf = sindico.Cpf;
            existing.Email = sindico.Email;
            existing.Telefone = sindico.Telefone;
            existing.Rua = sindico.Rua;
            existing.Numero = sindico.Numero;
            existing.Bairro = sindico.Bairro;
            existing.Cidade = sindico.Cidade;
            existing.Uf = sindico.Uf;
            existing.Cep = sindico.Cep;
            existing.Complemento = sindico.Complemento;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Sindicos.Find(id);
            if (existing == null) throw new KeyNotFoundException("Síndico não encontrado.");

            _context.Sindicos.Remove(existing);
            _context.SaveChanges();
        }

        public Sindico GetById(int id)
        {
            var s = _context.Sindicos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (s == null) throw new KeyNotFoundException("Síndico não encontrado.");
            return s;
        }

        public List<Sindico> GetAll()
        {
            return _context.Sindicos.AsNoTracking().ToList();
        }
    }
}