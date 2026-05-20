using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class AreaDeLazerServiceTests
    {
        private CondosmartContext context = null!;
        private AreaDeLazerService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            context = new CondosmartContext(options);
            service = new AreaDeLazerService(context);

            // Seed data
            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Dispose();
        }

        [TestMethod]
        public void CreateTest_ComDadosValidos_CriaAreaDeLazerComSucesso()
        {
            // Arrange
            var areaDeLazer = new AreaDeLazer
            {
                Nome = "Quadra de Tênis",
                Descricao = "Quadra coberta para tênis",
                Disponibilidade = true,
                CondominioId = 1,
                SindicoId = 1
            };

            // Act
            int id = service.Create(areaDeLazer);

            // Assert
            Assert.IsTrue(id > 0);
            var ariaCreated = service.GetById(id);
            Assert.IsNotNull(ariaCreated);
            Assert.AreEqual("Quadra de Tênis", ariaCreated.Nome);
            Assert.AreEqual("Quadra coberta para tênis", ariaCreated.Descricao);
            Assert.IsTrue(ariaCreated.Disponibilidade);
        }

        [TestMethod]
        public void CreateTest_ComNomeVazio_LancaArgumentException()
        {
            // Arrange
            var areaDeLazer = new AreaDeLazer
            {
                Nome = "",
                Descricao = "Descrição válida",
                Disponibilidade = true,
                CondominioId = 1,
                SindicoId = 1
            };

            // Act & Assert
            try
            {
                service.Create(areaDeLazer);
                Assert.Fail("Deveria ter lançado ArgumentException");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("obrigat"));
            }
        }

        [TestMethod]
        public void EditTest_ComDadosValidos_AtualizaAreaDeLazerComSucesso()
        {
            // Arrange
            var areaDeLazer = service.GetById(1)!;
            areaDeLazer.Nome = "Piscina Olímpica Atualizada";
            areaDeLazer.Descricao = "Piscina de 50 metros";
            areaDeLazer.Disponibilidade = false;

            // Act
            service.Edit(areaDeLazer);

            // Assert
            var updated = service.GetById(1);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Piscina Olímpica Atualizada", updated.Nome);
            Assert.AreEqual("Piscina de 50 metros", updated.Descricao);
            Assert.IsFalse(updated.Disponibilidade);
        }

        [TestMethod]
        public void DeleteTest_ComIdValido_RemoveAreaDeLazerComSucesso()
        {
            // Arrange
            var areaId = 2;
            var beforeDelete = service.GetById(areaId);
            Assert.IsNotNull(beforeDelete);

            // Act
            service.Delete(areaId);

            // Assert
            var afterDelete = service.GetById(areaId);
            Assert.IsNull(afterDelete);
        }

        [TestMethod]
        public void GetByIdTest_ComIdExistente_RetornaAreaDeLazer()
        {
            // Act
            var result = service.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Piscina", result.Nome);
        }

        [TestMethod]
        public void GetByIdTest_ComIdInexistente_RetornaNull()
        {
            // Act
            var result = service.GetById(999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllTest_RetornaTodasAsAreasDeLazer()
        {
            // Act
            var result = service.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => a.Nome == "Piscina"));
            Assert.IsTrue(result.Any(a => a.Nome == "Sauna"));
        }

        private void SeedTestData()
        {
            var condominio = new Condominio
            {
                Id = 1,
                Nome = "Condomínio Teste",
                Cnpj = "12345678000195"
            };
            context.Condominios.Add(condominio);

            var sindico = new Sindico
            {
                Id = 1,
                Cpf = "12345678900",
                Nome = "Síndico Teste",
                Email = "sindico@teste.com"
            };
            context.Sindicos.Add(sindico);

            var areaDeLazer1 = new AreaDeLazer
            {
                Id = 1,
                Nome = "Piscina",
                Descricao = "Piscina aquecida",
                Disponibilidade = true,
                CondominioId = 1,
                SindicoId = 1,
                CreatedAt = DateTime.Now
            };

            var areaDeLazer2 = new AreaDeLazer
            {
                Id = 2,
                Nome = "Sauna",
                Descricao = "Sauna seca",
                Disponibilidade = false,
                CondominioId = 1,
                SindicoId = 1,
                CreatedAt = DateTime.Now
            };

            context.AreaDeLazer.AddRange(areaDeLazer1, areaDeLazer2);
            context.SaveChanges();
        }
    }
}
