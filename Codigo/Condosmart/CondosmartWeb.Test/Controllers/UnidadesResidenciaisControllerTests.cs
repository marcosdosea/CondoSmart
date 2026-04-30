using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Mappers;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class UnidadesResidenciaisControllerTests
    {
        private static UnidadesResidenciaisController controller = null!;
        private static Mock<ICondominioService> mockCondominioService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IUnidadesResidenciaisService>();
            mockCondominioService = new Mock<ICondominioService>();
            var mockCepService = new Mock<ICepService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new UnidadeResidencialProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestUnidades());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetUnidade());

            mockService.Setup(s => s.Edit(It.IsAny<UnidadesResidenciais>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<UnidadesResidenciais>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            mockCondominioService.Setup(s => s.GetAll())
                .Returns(GetTestCondominios());

            mockCepService.Setup(s => s.IsValidAsync(It.IsAny<string?>()))
                .ReturnsAsync(true);

            controller = new UnidadesResidenciaisController(
                mockService.Object,
                mockCondominioService.Object,
                mockCepService.Object,
                mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PagedListViewModel<UnidadeResidencialViewModel>));
            var lista = (PagedListViewModel<UnidadeResidencialViewModel>)viewResult.ViewData.Model;

            Assert.AreEqual(3, lista.TotalItems);
            Assert.AreEqual(3, lista.Items.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UnidadeResidencialViewModel));
            var model = (UnidadeResidencialViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Bloco A - Apto 101", model.Identificador);
            Assert.AreEqual("Rua das Flores", model.Rua);
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
            var result = await controller.Create(GetNewUnidadeModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Identificador", "Campo requerido");

            var result = await controller.Create(GetNewUnidadeModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditTest_Get_Valid()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UnidadeResidencialViewModel));
            var model = (UnidadeResidencialViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Bloco A - Apto 101", model.Identificador);
            Assert.AreEqual("Rua das Flores", model.Rua);
        }

        [TestMethod]
        public async Task EditTest_Post_Valid()
        {
            var result = await controller.Edit(GetTargetUnidadeModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UnidadeResidencialViewModel));
            var model = (UnidadeResidencialViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Bloco A - Apto 101", model.Identificador);
        }

        [TestMethod]
        public void DeleteTest_Post_Valid()
        {
            var result = controller.DeleteConfirmed(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        private static UnidadesResidenciais GetTargetUnidade()
        {
            return new UnidadesResidenciais
            {
                Id = 1,
                Identificador = "Bloco A - Apto 101",
                Rua = "Rua das Flores",
                Numero = "100",
                Bairro = "Centro",
                Complemento = "Proximo ao mercado",
                Cep = "12345678",
                Cidade = "Sao Paulo",
                Uf = "SP",
                TelefoneResidencial = "1133334444",
                TelefoneCelular = "11987654321",
                MoradorId = 1,
                CondominioId = 1,
                SindicoId = 1,
                Condominio = new Condominio
                {
                    Id = 1,
                    Nome = "Condominio Alfa"
                },
                Morador = new Morador
                {
                    Id = 1,
                    Nome = "Joao Silva"
                }
            };
        }

        private UnidadeResidencialViewModel GetTargetUnidadeModel()
        {
            return new UnidadeResidencialViewModel
            {
                Id = 1,
                Identificador = "Bloco A - Apto 101",
                Rua = "Rua das Flores",
                Numero = "100",
                Bairro = "Centro",
                Complemento = "Proximo ao mercado",
                Cep = "12345678",
                Cidade = "Sao Paulo",
                Uf = "SP",
                TelefoneResidencial = "1133334444",
                TelefoneCelular = "11987654321",
                MoradorId = 1,
                CondominioId = 1,
                SindicoId = 1
            };
        }

        private UnidadeResidencialViewModel GetNewUnidadeModel()
        {
            return new UnidadeResidencialViewModel
            {
                Id = 99,
                Identificador = "Bloco B - Apto 202",
                Rua = "Rua das Acacias",
                Numero = "200",
                Bairro = "Jardim Paulista",
                Complemento = "Proximo ao shopping",
                Cep = "87654321",
                Cidade = "Sao Paulo",
                Uf = "SP",
                TelefoneResidencial = "1144445555",
                TelefoneCelular = "11912345678",
                MoradorId = 2,
                CondominioId = 1,
                SindicoId = 1
            };
        }

        private List<UnidadesResidenciais> GetTestUnidades()
        {
            return new List<UnidadesResidenciais>
            {
                new UnidadesResidenciais
                {
                    Id = 1,
                    Identificador = "Bloco A - Apto 101",
                    Rua = "Rua das Flores",
                    Numero = "100",
                    Bairro = "Centro",
                    Cep = "12345678",
                    Cidade = "Sao Paulo",
                    Uf = "SP",
                    TelefoneResidencial = "1133334444",
                    TelefoneCelular = "11987654321",
                    CondominioId = 1,
                    Condominio = new Condominio
                    {
                        Id = 1,
                        Nome = "Condominio Alfa"
                    },
                    Morador = new Morador
                    {
                        Id = 1,
                        Nome = "Joao Silva"
                    }
                },
                new UnidadesResidenciais
                {
                    Id = 2,
                    Identificador = "Bloco A - Apto 102",
                    Rua = "Rua das Flores",
                    Numero = "100",
                    Bairro = "Centro",
                    Cep = "12345678",
                    Cidade = "Sao Paulo",
                    Uf = "SP",
                    TelefoneResidencial = "1133335555",
                    TelefoneCelular = "11987654322",
                    CondominioId = 1,
                    Condominio = new Condominio
                    {
                        Id = 1,
                        Nome = "Condominio Alfa"
                    },
                    Morador = new Morador
                    {
                        Id = 2,
                        Nome = "Maria Santos"
                    }
                },
                new UnidadesResidenciais
                {
                    Id = 3,
                    Identificador = "Bloco B - Apto 201",
                    Rua = "Rua das Acacias",
                    Numero = "200",
                    Bairro = "Jardim Paulista",
                    Cep = "87654321",
                    Cidade = "Sao Paulo",
                    Uf = "SP",
                    TelefoneResidencial = "1144446666",
                    TelefoneCelular = "11987654323",
                    CondominioId = 2,
                    Condominio = new Condominio
                    {
                        Id = 2,
                        Nome = "Condominio Beta"
                    },
                    Morador = new Morador
                    {
                        Id = 3,
                        Nome = "Carlos Souza"
                    }
                }
            };
        }

        private List<Condominio> GetTestCondominios()
        {
            return new List<Condominio>
            {
                new Condominio
                {
                    Id = 1,
                    Nome = "Condominio Alfa",
                    Cnpj = "12345678901234",
                    Rua = "Rua A",
                    Numero = "10",
                    Bairro = "Centro",
                    Cidade = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Unidades = 10
                },
                new Condominio
                {
                    Id = 2,
                    Nome = "Condominio Beta",
                    Cnpj = "22345678901234",
                    Rua = "Rua B",
                    Numero = "20",
                    Bairro = "Bairro B",
                    Cidade = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Unidades = 15
                }
            };
        }
    }
}
