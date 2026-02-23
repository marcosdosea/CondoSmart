using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace Service
{
    public class MensalidadeService : IMensalidadeService
    {
        private readonly CondosmartContext context;

        public MensalidadeService(CondosmartContext context)
        {
            this.context = context;
        }

        public int Create(Mensalidade mensalidade)
        {
            context.Add(mensalidade);
            context.SaveChanges();
            return mensalidade.Id;
        }

        public void Edit(Mensalidade mensalidade)
        {
            context.Update(mensalidade);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var mensalidade = GetById(id);
            if (mensalidade != null)
            {
                context.Remove(mensalidade);
                context.SaveChanges();
            }
        }

        public Mensalidade? GetById(int id)
        {
            return context.Mensalidades?
                .Include(m => m.Condominio)
                .Include(m => m.Morador)
                .Include(m => m.Unidade)
                .Include(m => m.Pagamento)
                .FirstOrDefault(m => m.Id == id);
        }

        public List<Mensalidade> GetAll()
        {
            return context.Mensalidades?
                .Include(m => m.Condominio)
                .Include(m => m.Morador)
                .Include(m => m.Unidade)
                .Include(m => m.Pagamento)
                .ToList() ?? new List<Mensalidade>();
        }
    }
}
