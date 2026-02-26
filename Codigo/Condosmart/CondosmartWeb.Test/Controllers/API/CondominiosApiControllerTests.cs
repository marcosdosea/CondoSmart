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
    public class CondominiosApiControllerTests
    {
        private static CondominiosController controller = null!;
        private static Mock<ICondominioService> mockService = null!;
        private static IMapper mapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ICondominioService>();

            mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CondominioProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestCondominios());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetCondominio());

            mockService.Setup(s => s.GetById(99))
                .Returns((Condominio?)null);

            mockService.Setup(s => s.Create(It.IsAny<Condominio>()))
                .Returns(10);

            mockService.Setup(s => s.Edit(It.IsAny<Condominio>()))
                .Verifiable();

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new CondominiosController(mockService.Object, mapper);
        }

        // ---- GET /api/condominios ----

        [TestMethod]
        public void GetAll_Valido_Retorna200ComLista()
        {
            var result = controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<CondominioViewModel>));
            var lista = (List<CondominioViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // ---- GET /api/condominios/{id} ----

        [TestMethod]
        public void GetById_Valido_Retorna200ComCondominio()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(CondominioViewModel));
            var model = (CondominioViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Condomínio Beija Flor", model.Nome);
            Assert.AreEqual("12345678901234", model.Cnpj);
            Assert.AreEqual("contato@beijaflor.com.br", model.Email);
        }

        [TestMethod]
        public void GetById_NaoEncontrado_Retorna404()
        {
            var result = controller.GetById(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- POST /api/condominios ----

        [TestMethod]
        public void Create_Valido_Retorna201Created()
        {
            var result = controller.Create(GetNewCondominioModel());

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public void Create_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewCondominioModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Create_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<ICondominioService>();
            mockLocal.Setup(s => s.Create(It.IsAny<Condominio>()))
                .Throws(new ArgumentException("Nome do condomínio é obrigatório."));

            var controllerLocal = new CondominiosController(mockLocal.Object, mapper);
            var vm = GetNewCondominioModel();
            vm.Nome = string.Empty;

            var result = controllerLocal.Create(vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- PUT /api/condominios/{id} ----

        [TestMethod]
        public void Edit_Valido_Retorna200ComCondominio()
        {
            var vm = GetTargetCondominioModel();

            var result = controller.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(CondominioViewModel));
            var model = (CondominioViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Condomínio Beija Flor", model.Nome);
        }

        [TestMethod]
        public void Edit_IdDivergente_Retorna400()
        {
            var result = controller.Edit(99, GetTargetCondominioModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Edit_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Cnpj", "Campo requerido");

            var result = controller.Edit(1, GetTargetCondominioModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Edit_NaoEncontrado_Retorna404()
        {
            var vm = GetTargetCondominioModel();
            vm.Id = 99;

            var result = controller.Edit(99, vm);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Edit_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<ICondominioService>();
            mockLocal.Setup(s => s.GetById(1)).Returns(GetTargetCondominio());
            mockLocal.Setup(s => s.Edit(It.IsAny<Condominio>()))
                .Throws(new ArgumentException("CNPJ inválido."));

            var controllerLocal = new CondominiosController(mockLocal.Object, mapper);
            var vm = GetTargetCondominioModel();
            vm.Cnpj = "00000000000000";

            var result = controllerLocal.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- DELETE /api/condominios/{id} ----

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

        private static Condominio GetTargetCondominio() => new()
        {
            Id = 1,
            Nome = "Condomínio Beija Flor",
            Cnpj = "12345678901234",
            Email = "contato@beijaflor.com.br",
            Telefone = "11987654321",
            Rua = "Rua das Flores",
            Numero = "100",
            Bairro = "Pinheiros",
            Cidade = "São Paulo",
            Uf = "SP",
            Cep = "01310100",
            CreatedAt = new DateTime(2025, 12, 1)
        };

        private static CondominioViewModel GetTargetCondominioModel() => new()
        {
            Id = 1,
            Nome = "Condomínio Beija Flor",
            Cnpj = "12345678901234",
            Email = "contato@beijaflor.com.br",
            Telefone = "11987654321",
            Rua = "Rua das Flores",
            Numero = "100",
            Bairro = "Pinheiros",
            Cidade = "São Paulo",
            Uf = "SP",
            Cep = "01310100"
        };

        private static CondominioViewModel GetNewCondominioModel() => new()
        {
            Nome = "Condomínio Primavera",
            Cnpj = "98765432109876",
            Email = "contato@primavera.com.br",
            Telefone = "11998765432",
            Rua = "Avenida Paulista",
            Numero = "1000",
            Bairro = "Bela Vista",
            Cidade = "São Paulo",
            Uf = "SP",
            Cep = "01310100"
        };

        private static List<Condominio> GetTestCondominios() => new()
        {
            GetTargetCondominio(),
            new Condominio
            {
                Id = 2,
                Nome = "Condomínio Primavera",
                Cnpj = "98765432109876",
                Email = "contato@primavera.com.br",
                Telefone = "11998765432",
                Rua = "Avenida Paulista",
                Numero = "1000",
                Bairro = "Bela Vista",
                Cidade = "São Paulo",
                Uf = "SP",
                Cep = "01310100",
                CreatedAt = DateTime.UtcNow
            },
            new Condominio
            {
                Id = 3,
                Nome = "Condomínio Verão",
                Cnpj = "55544433322211",
                Email = "contato@verao.com.br",
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
