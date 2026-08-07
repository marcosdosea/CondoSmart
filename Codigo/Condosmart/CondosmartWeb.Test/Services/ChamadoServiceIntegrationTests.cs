using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CondosmartWeb.Services.Tests
{
    [TestClass]
    public class ChamadoServiceIntegrationTests
    {
        private static CondosmartContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new CondosmartContext(options);
        }

        [TestMethod]
        public void Create_DeveSalvarChamadoNoBanco()
        {
            using var context = CriarContextoEmMemoria();
            var service = new global::Service.ChamadoService(context);

            var chamado = new Chamado
            {
                Descricao = "Vazamento no corredor",
                Status = "aberto",
                CondominioId = 1,
                DataChamado = DateTime.Now
            };

            var id = service.Create(chamado);

            var chamadoSalvo = context.Chamados.Find(id);
            Assert.IsNotNull(chamadoSalvo);
            Assert.AreEqual("Vazamento no corredor", chamadoSalvo.Descricao);
            Assert.AreEqual("aberto", chamadoSalvo.Status);
        }

        [TestMethod]
        public void Edit_DeveAtualizarChamadoNoBanco()
        {
            using var context = CriarContextoEmMemoria();
            var service = new global::Service.ChamadoService(context);

            var chamado = new Chamado
            {
                Descricao = "Porta quebrada",
                Status = "aberto",
                CondominioId = 1,
                DataChamado = DateTime.Now
            };

            var id = service.Create(chamado);

            var chamadoAtualizado = service.GetById(id)!;
            chamadoAtualizado.Descricao = "Porta consertada";
            chamadoAtualizado.Status = "resolvido";

            service.Edit(chamadoAtualizado);

            var resultado = context.Chamados.Find(id);
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Porta consertada", resultado.Descricao);
            Assert.AreEqual("resolvido", resultado.Status);
        }

        [TestMethod]
        public void Delete_DeveRemoverChamadoDoBanco()
        {
            using var context = CriarContextoEmMemoria();
            var service = new global::Service.ChamadoService(context);

            var chamado = new Chamado
            {
                Descricao = "Luz queimada",
                Status = "aberto",
                CondominioId = 1,
                DataChamado = DateTime.Now
            };

            var id = service.Create(chamado);

            service.Delete(id);

            var resultado = context.Chamados.Find(id);
            Assert.IsNull(resultado);
        }
    }
}
