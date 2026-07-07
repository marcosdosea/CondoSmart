using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using Core.Data;
using System.Linq;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class PagamentoServiceTests
    {
        private CondosmartContext _context = null!;
        private PagamentoService _service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new CondosmartContext(options);
            _service = new PagamentoService(_context);
        }

        [TestCleanup]
        public void Cleanup() => _context?.Dispose();

        // 1. Testa a criação com sucesso
        [TestMethod]
        public void Create_ComDadosValidos_SalvaNoBanco()
        {
            var novoPagamento = new Pagamento 
            { 
                Valor = 350m, 
                FormaPagamento = "Cartao", 
                Status = "PENDENTE", 
                CondominioId = 1 
            };

            int idGerado = _service.Create(novoPagamento);

            Assert.IsTrue(idGerado > 0);
            var buscarNoBanco = _context.Pagamentos.Find(idGerado);
            Assert.IsNotNull(buscarNoBanco);
            Assert.AreEqual(350m, buscarNoBanco.Valor);
        }

        // 2. Testa a funcionalidade de Edição
        [TestMethod]
        public void Edit_ComDadosValidos_AtualizaNoBanco()
        {
            var pagamentoInicial = new Pagamento 
            { 
                Valor = 100m, 
                FormaPagamento = "Dinheiro", 
                Status = "PENDENTE", 
                CondominioId = 1 
            };
            _context.Pagamentos.Add(pagamentoInicial);
            _context.SaveChanges();

            _context.Entry(pagamentoInicial).State = EntityState.Detached;

            pagamentoInicial.Status = "PAGO";
            pagamentoInicial.FormaPagamento = "PIX";
            
            _service.Edit(pagamentoInicial);

            var pagamentoAtualizado = _context.Pagamentos.Find(pagamentoInicial.Id);
            Assert.IsNotNull(pagamentoAtualizado);
            Assert.AreEqual("PAGO", pagamentoAtualizado.Status);
            Assert.AreEqual("PIX", pagamentoAtualizado.FormaPagamento);
        }

        // 3. Testa a criação de múltiplos registros e contagem no banco
        [TestMethod]
        public void Create_MultiplosPagamentos_AumentaContagemNoBanco()
        {
            var p1 = new Pagamento { Valor = 100m, FormaPagamento = "PIX", Status = "PAGO", CondominioId = 1 };
            var p2 = new Pagamento { Valor = 200m, FormaPagamento = "Boleto", Status = "PENDENTE", CondominioId = 1 };

            _service.Create(p1);
            _service.Create(p2);

            int quantidadeNoBanco = _context.Pagamentos.Count();
            
            // Valida se o banco salvou e agora tem pelo menos 2 registross
            Assert.IsTrue(quantidadeNoBanco >= 2);
        }
    }
}