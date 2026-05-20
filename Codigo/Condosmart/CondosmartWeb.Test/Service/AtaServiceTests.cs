using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class AtaServiceTests
    {
        private CondosmartContext context = null!;
        private AtaService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            context = new CondosmartContext(options);
            service = new AtaService(context);

            context.Condominios.Add(new Condominio { Id = 1, Nome = "Condominio Teste", Cnpj = "12345678000195" });
            context.Atas.Add(new Ata { Id = 1, Titulo = "Ata Teste", DataReuniao = new DateOnly(2026, 1, 15), CondominioId = 1 });
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup() => context?.Dispose();

        [TestMethod]
        public void CreateTest_ComDadosValidos_CriaAta()
        {
            var ata = new Ata { Titulo = "Nova Ata", DataReuniao = new DateOnly(2026, 5, 10), CondominioId = 1 };

            int id = service.Create(ata);

            Assert.IsTrue(id > 0);
            Assert.AreEqual("Nova Ata", service.GetById(id)!.Titulo);
        }

        [TestMethod]
        public void GetByIdTest_ComIdExistente_RetornaAta()
        {
            var result = service.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Ata Teste", result.Titulo);
        }

        [TestMethod]
        public void DeleteTest_ComIdValido_RemoveAta()
        {
            service.Delete(1);

            Assert.IsNull(service.GetById(1));
        }
    }
}
