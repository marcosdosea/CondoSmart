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
            // Arrange
            var mockService = new Mock<IUnidadesResidenciaisService>();
            mockCondominioService = new Mock<ICondominioService>();

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

            controller = new UnidadesResidenciaisController(mockService.Object, mockCondominioService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<UnidadeResidencialViewModel>));
            var lista = (List<UnidadeResidencialViewModel>)viewResult.ViewData.Model;

            Assert.HasCount(3, lista);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
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
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CreateTest_Post_Valid()
        {
            // Act
            var result = controller.Create(GetNewUnidadeModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CreateTest_Post_Invalid()
        {
            // Arrange
            controller.ModelState.AddModelError("Identificador", "Campo requerido");

            // Act
            var result = controller.Create(GetNewUnidadeModel());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditTest_Get_Valid()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UnidadeResidencialViewModel));
            var model = (UnidadeResidencialViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Bloco A - Apto 101", model.Identificador);
            Assert.AreEqual("Rua das Flores", model.Rua);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(GetTargetUnidadeModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Get_Valid()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UnidadeResidencialViewModel));
            var model = (UnidadeResidencialViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Bloco A - Apto 101", model.Identificador);
        }

        [TestMethod]
        public void DeleteTest_Post_Valid()
        {
            // Act
            var result = controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        // --------- Dados de Teste ---------

        private static UnidadesResidenciais GetTargetUnidade()
        {
            return new UnidadesResidenciais
            {
                Id = 1,
                Identificador = "Bloco A - Apto 101",
                Rua = "Rua das Flores",
                Numero = "100",
                Bairro = "Centro",
                Complemento = "Próximo ao mercado",
                Cep = "12345678",
                Cidade = "São Paulo",
                Uf = "SP",
                TelefoneResidencial = "1133334444",
                TelefoneCelular = "11987654321",
                MoradorId = 1,
                CondominioId = 1,
                SindicoId = 1,
                Condominio = new Condominio
                {
                    Id = 1,
                    Nome = "Condomínio Alfa"
                },
                Morador = new Morador
                {
                    Id = 1,
                    Nome = "João Silva"
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
                Complemento = "Próximo ao mercado",
                Cep = "12345678",
                Cidade = "São Paulo",
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
                Rua = "Rua das Acácias",
                Numero = "200",
                Bairro = "Jardim Paulista",
                Complemento = "Próximo ao shopping",
                Cep = "87654321",
                Cidade = "São Paulo",
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
                    Cidade = "São Paulo",
                    Uf = "SP",
                    TelefoneResidencial = "1133334444",
                    TelefoneCelular = "11987654321",
                    CondominioId = 1,
                    Condominio = new Condominio
                    {
                        Id = 1,
                        Nome = "Condomínio Alfa"
                    },
                    Morador = new Morador
                    {
                        Id = 1,
                        Nome = "João Silva"
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
                    Cidade = "São Paulo",
                    Uf = "SP",
                    TelefoneResidencial = "1133335555",
                    TelefoneCelular = "11987654322",
                    CondominioId = 1,
                    Condominio = new Condominio
                    {
                        Id = 1,
                        Nome = "Condomínio Alfa"
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
                    Rua = "Rua das Acácias",
                    Numero = "200",
                    Bairro = "Jardim Paulista",
                    Cep = "87654321",
                    Cidade = "São Paulo",
                    Uf = "SP",
                    TelefoneResidencial = "1144446666",
                    TelefoneCelular = "11987654323",
                    CondominioId = 2,
                    Condominio = new Condominio
                    {
                        Id = 2,
                        Nome = "Condomínio Beta"
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
                    Nome = "Condomínio Alfa",
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
                    Nome = "Condomínio Beta",
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
