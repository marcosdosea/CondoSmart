using Core.DTO;
using Core.Models;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Service;
using System;
using Xunit;

namespace CondosmartWeb.Tests.Services
{
    public class PagamentoServiceTests
    {
        [Fact]
        public void LiquidarMensalidade_PagamentoAtrasadoComValorInsuficiente_DeveLancarExcecao()
        {
            // 1. Arrange (Preparar o banco de dados em memória e os dados fictícios)
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Banco novo para cada teste
                .Options;

            using (var context = new CondosmartContext(options))
            {
                // Simulando uma mensalidade de R$ 100,00 vencida há 10 dias
                var mensalidade = new Mensalidade
                {
                    Id = 1,
                    Valor = 100.00m,
                    Vencimento = DateTime.Now.AddDays(-10),
                    Status = "PENDENTE",
                    CondominioId = 1,
                    UnidadeId = 1,
                    MoradorId = 1
                };
                context.Mensalidades.Add(mensalidade);
                context.SaveChanges();

                var service = new PagamentoService(context);

                // Morador "espertinho" tentando pagar só R$ 100,00 sem os juros
                var dto = new LiquidarMensalidadeDTO
                {
                    MensalidadeId = 1,
                    ValorPago = 100.00m,
                    DataPagamento = DateTime.Now,
                    FormaPagamento = "PIX"
                };

                // 2. Act & 3. Assert (Agir e Verificar)
                // O teste PASSA se o sistema PROIBIR o pagamento e lançar a nossa ArgumentException
                var exception = Xunit.Assert.Throws<ArgumentException>(() => service.LiquidarMensalidade(dto));
                Xunit.Assert.Contains("Valor insuficiente", exception.Message);
            }
        }
    }
}