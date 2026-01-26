using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class CondominioService : ICondominioService
    {
        private readonly CondosmartContext _context;

        public CondominioService(CondosmartContext context)
        {
            _context = context;
        }

        public int Create(Condominio condominio)
        {
            _context.Condominios.Add(condominio);
            _context.SaveChanges();
            return condominio.Id;
        }

        public void Edit(Condominio condominio)
        {
            var existing = _context.Condominios.Find(condominio.Id);
            if (existing == null) throw new KeyNotFoundException("Condomínio não encontrado.");

            existing.Nome = condominio.Nome;
            existing.Cnpj = condominio.Cnpj;
            existing.Rua = condominio.Rua;
            existing.Numero = condominio.Numero;
            existing.Bairro = condominio.Bairro;
            existing.Cidade = condominio.Cidade;
            existing.Uf = condominio.Uf;
            existing.Cep = condominio.Cep;
            existing.Email = condominio.Email;
            existing.Telefone = condominio.Telefone;
            existing.Complemento = condominio.Complemento;
            existing.Unidades = condominio.Unidades;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Condominios.Find(id);
            if (existing == null) throw new KeyNotFoundException("Condomínio não encontrado.");

            _context.Condominios.Remove(existing);
            _context.SaveChanges();
        }

        public Condominio GetById(int id)
        {
            var c = _context.Condominios.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (c == null) throw new KeyNotFoundException("Condomínio não encontrado.");
            return c;
        }

        public List<Condominio> GetAll()
        {
            return _context.Condominios.AsNoTracking().ToList();
        }
    }
}
