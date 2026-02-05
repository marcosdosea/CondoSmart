using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly CondosmartContext _context;

        public PagamentoService(CondosmartContext context)
        {
            _context = context;
        }

        public int Create(Pagamento pagamento)
        {
            if (pagamento.DataPagamento.HasValue && pagamento.Status == "Pendente")
            {
                pagamento.Status = "Pago";
            }

            _context.Add(pagamento);
            _context.SaveChanges();
            return pagamento.Id;
        }

        public void Edit(Pagamento pagamento)
        {
            _context.Update(pagamento);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var pagamento = _context.Pagamentos.Find(id);
            if (pagamento != null)
            {
                _context.Remove(pagamento);
                _context.SaveChanges();
            }
        }

        public Pagamento? Get(int id)
        {
            return _context.Pagamentos.Find(id);
        }

        public IEnumerable<Pagamento> GetAll()
        {
            return _context.Pagamentos.AsNoTracking().ToList();
        }
    }
}