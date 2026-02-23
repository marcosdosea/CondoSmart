using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly CondosmartContext context;

        public PagamentoService(CondosmartContext context)
        {
            this.context = context;
        }

        public int Create(Pagamento pagamento)
        {
            context.Add(pagamento);
            context.SaveChanges();
            return pagamento.Id;
        }

        public void Edit(Pagamento pagamento)
        {
            context.Update(pagamento);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var pagamento = GetById(id);
            if (pagamento != null)
            {
                context.Remove(pagamento);
                context.SaveChanges();
            }
        }

        public Pagamento? GetById(int id)
        {
            return context.Pagamentos?
                .Include(p => p.Condominio)
                .Include(p => p.Morador)
                .Include(p => p.Unidade)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Pagamento> GetAll()
        {
            return context.Pagamentos?
                .Include(p => p.Condominio)
                .Include(p => p.Morador)
                .Include(p => p.Unidade)
                .ToList() ?? new List<Pagamento>();
        }
    }
}
