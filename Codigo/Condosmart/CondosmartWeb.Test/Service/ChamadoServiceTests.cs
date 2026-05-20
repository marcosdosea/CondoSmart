using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class ChamadoServiceTests
    {
        private CondosmartContext context = null!;
        private ChamadoService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            context = new CondosmartContext(options);
            service = new ChamadoService(context);

            context.Condominios.Add(new Condominio { Id = 1, Nome = "Condominio Teste", Cnpj = "12345678000195" });
            context.Chamados.Add(new Chamado { Id = 1, Descricao = "Chamado Teste", Status = "aberto", CondominioId = 1 });
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup() => context?.Dispose();

        [TestMethod]
        public void CreateTest_ComDadosValidos_CriaChamado()
        {
            var chamado = new Chamado { Descricao = "Novo Chamado", Status = "aberto", CondominioId = 1 };

            int id = service.Create(chamado);

            Assert.IsTrue(id > 0);
            Assert.AreEqual("Novo Chamado", service.GetById(id)!.Descricao);
        }

        [TestMethod]
        public void GetAllTest_RetornaTodosOsChamados()
        {
            var result = service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void DeleteTest_ComIdValido_RemoveChamado()
        {
            service.Delete(1);

            Assert.IsNull(service.GetById(1));
        }
    }
}
