using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class VisitanteServiceTests
    {
        private CondosmartContext context = null!;
        private VisitanteService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            context = new CondosmartContext(options);
            service = new VisitanteService(context);

            context.Moradores.Add(new Morador { Id = 1, Nome = "Morador Teste", Cpf = "11111111111", Email = "m@t.com" });
            context.Visitantes.Add(new Visitantes { Id = 1, Nome = "Visitante 1", Telefone = "11999999999", MoradorId = 1 });
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup() => context?.Dispose();

        [TestMethod]
        public void CreateTest_ComDadosValidos_CriaVisitante()
        {
            var visitante = new Visitantes { Nome = "Novo Visitante", Telefone = "11888888888", MoradorId = 1 };

            int id = service.Create(visitante);

            Assert.IsTrue(id > 0);
            Assert.AreEqual("Novo Visitante", service.GetById(id)!.Nome);
        }

        [TestMethod]
        public void GetByIdTest_ComIdExistente_RetornaVisitante()
        {
            var result = service.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Visitante 1", result.Nome);
        }

        [TestMethod]
        public void DeleteTest_ComIdValido_RemoveVisitante()
        {
            service.Delete(1);

            Assert.IsNull(service.GetById(1));
        }
    }
}
