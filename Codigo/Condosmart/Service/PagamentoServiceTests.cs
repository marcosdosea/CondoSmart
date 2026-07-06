using Core.Data;
using Core.DTO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
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
            // Configura o banco de dados em memória, gerando um nome único para cada teste
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new CondosmartContext(options);
            _service = new PagamentoService(_context);

            // Popula o banco em memória com dados iniciais para os testes
            _context.Mensalidades.Add(new Mensalidade 
            { 
                Id = 1, 
                Valor = 1000m, 
                Vencimento = DateTime.Now.Date, 
                Status = "PENDENTE",
                CondominioId = 1,
                UnidadeId = 1,
                MoradorId = 1
            });

            _context.Mensalidades.Add(new Mensalidade 
            { 
                Id = 2, 
                Valor = 1000m, 
                Vencimento = DateTime.Now.Date.AddDays(-10), // Atrasada em 10 dias
                Status = "PENDENTE",
                CondominioId = 1,
                UnidadeId = 1,
                MoradorId = 1
            });
            
            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup() => _context?.Dispose();

        [TestMethod]
        public void LiquidarMensalidade_EmDia_GeraPagamentoEAtualizaStatus()
        {
            // Arrange
            var dto = new LiquidarMensalidadeDTO 
            {
                MensalidadeId = 1, // Mensalidade em dia
                ValorPago = 1000m,
                DataPagamento = DateTime.Now.Date,
                FormaPagamento = "PIX"
            };

            // Act
            _service.LiquidarMensalidade(dto);

            // Assert
            var mensalidadeAtualizada = _context.Mensalidades.Find(1);
            Assert.AreEqual("PAGO", mensalidadeAtualizada!.Status);
            Assert.IsNotNull(mensalidadeAtualizada.PagamentoId);
            
            var pagamentoGerado = _context.Pagamentos.Find(mensalidadeAtualizada.PagamentoId);
            Assert.IsNotNull(pagamentoGerado);
            Assert.AreEqual(1000m, pagamentoGerado.Valor);
            Assert.AreEqual("CONCLUIDO", pagamentoGerado.Status);
        }

        [TestMethod]
        public void LiquidarMensalidade_EmAtrasoComValorInsuficiente_LancaExcecao()
        {
            // Arrange
            var dto = new LiquidarMensalidadeDTO 
            {
                MensalidadeId = 2, // 10 dias atrasada (Multa de 20 + Juros de 3.30 = 1023.30 exigidos)
                ValorPago = 1000m, // Tentando pagar apenas o valor base (vai ser rejeitado)
                DataPagamento = DateTime.Now.Date,
                FormaPagamento = "Boleto"
            };

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => _service.LiquidarMensalidade(dto));
            
            // Verifica se a mensagem de erro da regra de negócio foi disparada
            StringAssert.Contains(exception.Message, "Valor insuficiente");
        }

        [TestMethod]
        public void LiquidarMensalidade_MensalidadeJaPaga_LancaExcecao()
        {
            // Arrange
            _context.Mensalidades.Add(new Mensalidade 
            { 
                Id = 3, 
                Valor = 500m, 
                Vencimento = DateTime.Now.Date,
                Status = "PAGO" // Já está paga
            });
            _context.SaveChanges();

            var dto = new LiquidarMensalidadeDTO 
            {
                MensalidadeId = 3,
                ValorPago = 500m,
                DataPagamento = DateTime.Now.Date,
                FormaPagamento = "PIX"
            };

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => _service.LiquidarMensalidade(dto));
            StringAssert.Contains(exception.Message, "já encontra-se quitada");
        }
    }
}