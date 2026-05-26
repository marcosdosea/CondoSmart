using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class ChamadoServiceIntegrationTests
    {
        private CondosmartContext context = null!;
        private ChamadoService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"IntegrationDb_{Guid.NewGuid()}")
                .Options;

            context = new CondosmartContext(options);
            service = new ChamadoService(context);

            context.Condominios.Add(new Condominio { Id = 1, Nome = "Condominio Integracao", Cnpj = "12345678000195" });
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup() => context?.Dispose();

        [TestMethod]
        public void Create_Integration_CriaChamadoNaBase()
        {
            var chamado = new Chamado { Descricao = "Chamado Integration", Status = "aberto", CondominioId = 1 };

            int id = service.Create(chamado);

            var fromDb = context.Chamados.Find(id);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("Chamado Integration", fromDb!.Descricao);
        }

        [TestMethod]
        public void Edit_Integration_AtualizaChamadoNaBase()
        {
            var chamado = new Chamado { Descricao = "ParaEditar", Status = "aberto", CondominioId = 1 };
            int id = service.Create(chamado);

            var criado = service.GetById(id)!;
            criado.Status = "resolvido";
            criado.Descricao = "Editado";
            service.Edit(criado);

            var after = context.Chamados.Find(id);
            Assert.IsNotNull(after);
            Assert.AreEqual("resolvido", after!.Status);
            Assert.AreEqual("Editado", after.Descricao);
        }

        [TestMethod]
        public void Delete_Integration_RemoveChamadoDaBase()
        {
            var chamado = new Chamado { Descricao = "ParaRemover", Status = "aberto", CondominioId = 1 };
            int id = service.Create(chamado);

            service.Delete(id);

            var after = context.Chamados.Find(id);
            Assert.IsNull(after);
        }
    }
}
