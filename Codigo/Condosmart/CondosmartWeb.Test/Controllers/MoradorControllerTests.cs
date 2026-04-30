using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Mappers;
using CondosmartWeb.Models;
using Core.DTO;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MoradorControllerTests
    {
        private static MoradorController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IMoradorService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockUnidadesService = new Mock<IUnidadesResidenciaisService>();
            var mockProvisionamentoService = new Mock<IMoradorProvisionamentoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new MoradorProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll()).Returns(GetTestMoradores());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetMorador());
            mockService.Setup(s => s.Edit(It.IsAny<Morador>())).Verifiable();
            mockService.Setup(s => s.Create(It.IsAny<Morador>())).Returns(10);
            mockService.Setup(s => s.Delete(It.IsAny<int>())).Verifiable();

            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio>());

            mockUnidadesService.Setup(s => s.GetAll()).Returns(new List<UnidadesResidenciais>
            {
                new() { Id = 7, Identificador = "A-101", CondominioId = 1, MoradorId = 1, Condominio = new Condominio { Id = 1, Nome = "Condominio Alfa" } },
                new() { Id = 8, Identificador = "B-202", CondominioId = 2, MoradorId = null, Condominio = new Condominio { Id = 2, Nome = "Condominio Beta" } }
            });

            mockProvisionamentoService
                .Setup(s => s.CadastrarComAcessoAsync(It.IsAny<Morador>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ProvisionamentoMoradorResultDTO
                {
                    MoradorId = 10,
                    NomeMorador = "Joao Santos",
                    Email = "joao@example.com",
                    SenhaTemporaria = "Condo@123A",
                    Condominio = "Condominio Beta",
                    Unidade = "B-202",
                    UrlAcesso = "https://localhost/login",
                    EmailEnviado = true
                });

            mockProvisionamentoService
                .Setup(s => s.AtualizarVinculoUnidadeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            mockProvisionamentoService
                .Setup(s => s.AtualizarContaMoradorAsync(It.IsAny<string?>(), It.IsAny<Morador>()))
                .Returns(Task.CompletedTask);

            mockProvisionamentoService
                .Setup(s => s.RemoverAcessoAsync(It.IsAny<int>(), It.IsAny<string?>()))
                .Returns(Task.CompletedTask);

            controller = new MoradorController(
                mockService.Object,
                mockCondominioService.Object,
                mockUnidadesService.Object,
                mockProvisionamentoService.Object,
                mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<MoradorViewModel>));
            var lista = (List<MoradorViewModel>)viewResult.ViewData.Model;

            Assert.HasCount(3, lista);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
            Assert.AreEqual(7, model.UnidadeId);
        }

        [TestMethod]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            var urlMock = new Mock<IUrlHelper>();
            urlMock.Setup(u => u.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://localhost/login");
            controller.Url = urlMock.Object;

            var result = await controller.Create(GetNewMoradorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = await controller.Create(GetNewMoradorModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditTest_Get_Valid()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
            Assert.AreEqual(7, model.UnidadeId);
        }

        [TestMethod]
        public async Task EditTest_Post_Valid()
        {
            var result = await controller.Edit(GetTargetMoradorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Get_Valid()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual(7, model.UnidadeId);
        }

        [TestMethod]
        public async Task DeleteTest_Post_Valid()
        {
            var result = await controller.DeleteConfirmed(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

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
                Cidade = "Sao Paulo",
                Uf = "SP",
                CondominioId = 1,
                CreatedAt = new DateTime(2024, 1, 15)
            };
        }

        private MoradorViewModel GetTargetMoradorModel()
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
                Cidade = "Sao Paulo",
                Uf = "SP",
                CondominioId = 1,
                UnidadeId = 7
            };
        }

        private MoradorViewModel GetNewMoradorModel()
        {
            return new MoradorViewModel
            {
                Nome = "Joao Santos",
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
                CondominioId = 2,
                UnidadeId = 8
            };
        }

        private List<Morador> GetTestMoradores()
        {
            return new List<Morador>
            {
                new()
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
                    Cidade = "Sao Paulo",
                    Uf = "SP",
                    CondominioId = 1,
                    CreatedAt = new DateTime(2024, 1, 15)
                },
                new()
                {
                    Id = 2,
                    Nome = "Joao Santos",
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
                new()
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
                    Cidade = "Brasilia",
                    Uf = "DF",
                    CondominioId = 2,
                    CreatedAt = new DateTime(2024, 3, 5)
                }
            };
        }
    }
}
