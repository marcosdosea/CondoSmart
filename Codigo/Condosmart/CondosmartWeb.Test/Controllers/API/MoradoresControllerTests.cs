using AutoMapper;
using CondosmartWeb.Controllers.API;
using CondosmartWeb.Mappers;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CondosmartWeb.Controllers.Tests.API
{
    [TestClass]
    public class MoradoresControllerTests
    {
        private static MoradoresController controller = null!;
        private static Mock<IMoradorService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IMoradorService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MoradorProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestMoradores());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetMorador());

            mockService.Setup(s => s.GetById(99))
                .Returns((Morador?)null);

            mockService.Setup(s => s.Edit(It.IsAny<Morador>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Morador>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new MoradoresController(mockService.Object, mapper);
        }

        // GET /api/moradores

        [TestMethod]
        public void GetAll_Retorna200_ComLista()
        {
            var result = controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<MoradorViewModel>));
            var lista = (List<MoradorViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // GET /api/moradores/{id}

        [TestMethod]
        public void GetById_Existente_Retorna200()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(MoradorViewModel));
            var model = (MoradorViewModel)ok.Value!;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public void GetById_NaoExistente_Retorna404()
        {
            var result = controller.GetById(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // POST /api/moradores

        [TestMethod]
        public void Create_Valido_Retorna201()
        {
            var result = controller.Create(GetNewMoradorViewModel());

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public void Create_Invalido_Retorna400()
        {
            controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório");

            var result = controller.Create(new MoradorViewModel { Cpf = "12345678901" });

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // PUT /api/moradores/{id}

        [TestMethod]
        public void Update_Valido_Retorna200()
        {
            var vm = GetTargetMoradorViewModel();

            var result = controller.Update(1, vm);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(MoradorViewModel));
        }

        [TestMethod]
        public void Update_IdDivergente_Retorna400()
        {
            var vm = GetTargetMoradorViewModel(); // vm.Id = 1

            var result = controller.Update(99, vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Update_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Cpf", "CPF deve ter 11 caracteres.");

            var result = controller.Update(1, GetTargetMoradorViewModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // DELETE /api/moradores/{id}

        [TestMethod]
        public void Delete_Existente_Retorna200()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockService.Verify(s => s.Delete(1), Times.Once);
        }

        [TestMethod]
        public void Delete_NaoExistente_Retorna404()
        {
            var result = controller.Delete(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockService.Verify(s => s.Delete(It.IsAny<int>()), Times.Never);
        }

        // --------- Dados de Teste ---------

        private static Morador GetTargetMorador()
        {
            return new Morador
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
        }

        private static MoradorViewModel GetTargetMoradorViewModel()
        {
            return new MoradorViewModel
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
        }

        private static MoradorViewModel GetNewMoradorViewModel()
        {
            return new MoradorViewModel
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
        }

        private static List<Morador> GetTestMoradores()
        {
            return new List<Morador>
            {
                new Morador { Id = 1, Nome = "Maria Silva",  Cpf = "12345678901", CondominioId = 1 },
                new Morador { Id = 2, Nome = "João Santos",  Cpf = "98765432101", CondominioId = 1 },
                new Morador { Id = 3, Nome = "Ana Costa",    Cpf = "11122233344", CondominioId = 2 }
            };
        }
    }
}
