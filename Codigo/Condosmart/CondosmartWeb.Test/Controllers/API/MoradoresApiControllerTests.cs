using AutoMapper;
using CondosmartAPI.Controllers;
using CondosmartAPI.Mappers;
using CondosmartAPI.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MoradoresApiControllerTests
    {
        private static MoradoresController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IMoradorService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MoradorProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestMoradores());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetMorador());

            mockService.Setup(s => s.GetById(99))
                .Returns((Morador?)null);

            mockService.Setup(s => s.Create(It.IsAny<Morador>()))
                .Returns(10);

            mockService.Setup(s => s.Edit(It.IsAny<Morador>()))
                .Verifiable();

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            mockUserManager
                .Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            controller = new MoradoresController(mockService.Object, mapper, mockUserManager.Object);
        }

        // ---- GET /api/moradores ----

        [TestMethod]
        public void GetAll_Valido_Retorna200ComLista()
        {
            // Act
            var result = controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<MoradorViewModel>));
            var lista = (List<MoradorViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // ---- GET /api/moradores/{id} ----

        [TestMethod]
        public void GetById_Valido_Retorna200ComMorador()
        {
            // Act
            var result = controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(MoradorViewModel));
            var model = (MoradorViewModel)ok.Value!;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public void GetById_NaoEncontrado_Retorna404()
        {
            // Act
            var result = controller.GetById(99);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- POST /api/moradores ----

        [TestMethod]
        public async Task Create_Valido_Retorna201Created()
        {
            // Act
            var result = await controller.Create(GetNewMoradorModel());

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public async Task Create_ModeloInvalido_Retorna400()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = await controller.Create(GetNewMoradorModel());

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- PUT /api/moradores/{id} ----

        [TestMethod]
        public void Edit_Valido_Retorna200ComMorador()
        {
            // Arrange
            var vm = GetTargetMoradorModel();

            // Act
            var result = controller.Edit(vm.Id, vm);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(MoradorViewModel));
            var model = (MoradorViewModel)ok.Value!;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public void Edit_IdDivergente_Retorna400()
        {
            // Act — ID da rota (99) diferente do ID do body (1)
            var result = controller.Edit(99, GetTargetMoradorModel());

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Edit_ModeloInvalido_Retorna400()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "Campo requerido");
            var vm = GetTargetMoradorModel();

            // Act
            var result = controller.Edit(vm.Id, vm);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- DELETE /api/moradores/{id} ----

        [TestMethod]
        public void Delete_Valido_Retorna200()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Delete_NaoEncontrado_Retorna404()
        {
            // Act
            var result = controller.Delete(99);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // --------- Dados de Teste ---------

        private static Morador GetTargetMorador() => new()
        {
            Id = 1,
            Nome = "Maria Silva",
            Cpf = "12345678901",
            Rg = "123456789",
            Telefone = "11987654321",
            Email = "maria@example.com",
            Rua = "Rua A",
            Bairro = "Centro",
            Numero = "123",
            Complemento = "Apto 101",
            Cep = "12345678",
            Cidade = "São Paulo",
            Uf = "SP",
            CondominioId = 1,
            CreatedAt = new DateTime(2024, 1, 15)
        };

        private static MoradorViewModel GetTargetMoradorModel() => new()
        {
            Id = 1,
            Nome = "Maria Silva",
            Cpf = "12345678901",
            Rg = "123456789",
            Telefone = "11987654321",
            Email = "maria@example.com",
            Rua = "Rua A",
            Bairro = "Centro",
            Numero = "123",
            Complemento = "Apto 101",
            Cep = "12345678",
            Cidade = "São Paulo",
            Uf = "SP",
            CondominioId = 1
        };

        private static MoradorViewModel GetNewMoradorModel() => new()
        {
            Nome = "João Santos",
            Cpf = "98765432101",
            Rg = "987654321",
            Telefone = "11912345678",
            Email = "joao@example.com",
            Rua = "Rua B",
            Bairro = "Vila",
            Numero = "456",
            Complemento = "Apto 202",
            Cep = "87654321",
            Cidade = "Rio de Janeiro",
            Uf = "RJ",
            CondominioId = 2
        };

        private static List<Morador> GetTestMoradores() => new()
        {
            new Morador
            {
                Id = 1,
                Nome = "Maria Silva",
                Cpf = "12345678901",
                Rg = "123456789",
                Telefone = "11987654321",
                Email = "maria@example.com",
                Rua = "Rua A",
                Bairro = "Centro",
                Numero = "123",
                Complemento = "Apto 101",
                Cep = "12345678",
                Cidade = "São Paulo",
                Uf = "SP",
                CondominioId = 1,
                CreatedAt = new DateTime(2024, 1, 15)
            },
            new Morador
            {
                Id = 2,
                Nome = "João Santos",
                Cpf = "98765432101",
                Rg = "987654321",
                Telefone = "11912345678",
                Email = "joao@example.com",
                Rua = "Rua B",
                Bairro = "Vila",
                Numero = "456",
                Complemento = "Apto 202",
                Cep = "87654321",
                Cidade = "Rio de Janeiro",
                Uf = "RJ",
                CondominioId = 1,
                CreatedAt = new DateTime(2024, 2, 10)
            },
            new Morador
            {
                Id = 3,
                Nome = "Ana Costa",
                Cpf = "55555555555",
                Rg = "555555555",
                Telefone = "11988888888",
                Email = "ana@example.com",
                Rua = "Rua C",
                Bairro = "Jardins",
                Numero = "789",
                Complemento = "Apto 303",
                Cep = "54321876",
                Cidade = "Brasília",
                Uf = "DF",
                CondominioId = 2,
                CreatedAt = new DateTime(2024, 3, 5)
            }
        };
    }
}
