using AutoMapper;
using CondosmartAPI.Controllers;
using CondosmartAPI.Mappers;
using CondosmartAPI.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class SindicosApiControllerTests
    {
        private static SindicosController controller = null!;
        private static Mock<ISindicoService> mockService = null!;
        private static IMapper mapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ISindicoService>();

            mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new SindicoProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestSindicos());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetSindico());

            mockService.Setup(s => s.GetById(99))
                .Returns((Sindico?)null);

            mockService.Setup(s => s.Create(It.IsAny<Sindico>()))
                .Returns(10);

            mockService.Setup(s => s.Edit(It.IsAny<Sindico>()))
                .Verifiable();

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new SindicosController(mockService.Object, mapper);
        }

        // ---- GET /api/sindicos ----

        [TestMethod]
        public void GetAll_Valido_Retorna200ComLista()
        {
            var result = controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<SindicoViewModel>));
            var lista = (List<SindicoViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // ---- GET /api/sindicos/{id} ----

        [TestMethod]
        public void GetById_Valido_Retorna200ComSindico()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(SindicoViewModel));
            var model = (SindicoViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("João Silva", model.Nome);
            Assert.AreEqual("11122233344", model.Cpf);
            Assert.AreEqual("joao@email.com", model.Email);
        }

        [TestMethod]
        public void GetById_NaoEncontrado_Retorna404()
        {
            var result = controller.GetById(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- POST /api/sindicos ----

        [TestMethod]
        public void Create_Valido_Retorna201Created()
        {
            var result = controller.Create(GetNewSindicoModel());

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public void Create_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewSindicoModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Create_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<ISindicoService>();
            mockLocal.Setup(s => s.Create(It.IsAny<Sindico>()))
                .Throws(new ArgumentException("Nome do síndico é obrigatório."));

            var controllerLocal = new SindicosController(mockLocal.Object, mapper);
            var vm = GetNewSindicoModel();
            vm.Nome = string.Empty;

            var result = controllerLocal.Create(vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- PUT /api/sindicos/{id} ----

        [TestMethod]
        public void Edit_Valido_Retorna200ComSindico()
        {
            var vm = GetTargetSindicoModel();

            var result = controller.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(SindicoViewModel));
            var model = (SindicoViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("João Silva", model.Nome);
        }

        [TestMethod]
        public void Edit_IdDivergente_Retorna400()
        {
            var result = controller.Edit(99, GetTargetSindicoModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Edit_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Cpf", "Campo requerido");

            var result = controller.Edit(1, GetTargetSindicoModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Edit_NaoEncontrado_Retorna404()
        {
            var vm = GetTargetSindicoModel();
            vm.Id = 99;

            var result = controller.Edit(99, vm);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Edit_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<ISindicoService>();
            mockLocal.Setup(s => s.GetById(1)).Returns(GetTargetSindico());
            mockLocal.Setup(s => s.Edit(It.IsAny<Sindico>()))
                .Throws(new ArgumentException("CPF inválido."));

            var controllerLocal = new SindicosController(mockLocal.Object, mapper);
            var vm = GetTargetSindicoModel();
            vm.Cpf = "00000000000";

            var result = controllerLocal.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- DELETE /api/sindicos/{id} ----

        [TestMethod]
        public void Delete_Valido_Retorna204NoContent()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void Delete_NaoEncontrado_Retorna404()
        {
            var result = controller.Delete(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // --------- Dados de Teste ---------

        private static Sindico GetTargetSindico() => new()
        {
            Id = 1,
            Nome = "João Silva",
            Cpf = "11122233344",
            Email = "joao@email.com",
            Telefone = "11987654321",
            Rua = "Rua dos Síndicos",
            Numero = "50",
            Bairro = "Vila Mariana",
            Cidade = "São Paulo",
            Uf = "SP",
            Cep = "01325010",
            CreatedAt = new DateTime(2025, 12, 1)
        };

        private static SindicoViewModel GetTargetSindicoModel() => new()
        {
            Id = 1,
            Nome = "João Silva",
            Cpf = "11122233344",
            Email = "joao@email.com",
            Telefone = "11987654321",
            Rua = "Rua dos Síndicos",
            Numero = "50",
            Bairro = "Vila Mariana",
            Cidade = "São Paulo",
            Uf = "SP",
            Cep = "01325010"
        };

        private static SindicoViewModel GetNewSindicoModel() => new()
        {
            Nome = "Maria Santos",
            Cpf = "55566677788",
            Email = "maria@email.com",
            Telefone = "11998765432",
            Rua = "Avenida Imigrantes",
            Numero = "1500",
            Bairro = "Santo Amaro",
            Cidade = "São Paulo",
            Uf = "SP",
            Cep = "04359000"
        };

        private static List<Sindico> GetTestSindicos() => new()
        {
            GetTargetSindico(),
            new Sindico
            {
                Id = 2,
                Nome = "Maria Santos",
                Cpf = "55566677788",
                Email = "maria@email.com",
                Telefone = "11998765432",
                Rua = "Avenida Imigrantes",
                Numero = "1500",
                Bairro = "Santo Amaro",
                Cidade = "São Paulo",
                Uf = "SP",
                Cep = "04359000",
                CreatedAt = DateTime.UtcNow
            },
            new Sindico
            {
                Id = 3,
                Nome = "Carlos Mota",
                Cpf = "99988877766",
                Email = "carlos@email.com",
                Telefone = "11999999999",
                Rua = "Rua do Comércio",
                Numero = "250",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP",
                Cep = "01310100",
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}
