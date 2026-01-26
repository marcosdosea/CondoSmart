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
    public class CondominioControllerTests
    {
        private static CondominioController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<ICondominioService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CondominioProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestCondominios());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetCondominio());

            mockService.Setup(s => s.Edit(It.IsAny<Condominio>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Condominio>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new CondominioController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<CondominioViewModel>));
            var lista = (List<CondominioViewModel>)viewResult.ViewData.Model;

            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CondominioViewModel));
            var model = (CondominioViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Condomínio Alfa", model.Nome);
            Assert.AreEqual("12345678901234", model.Cnpj);
            Assert.AreEqual("SP", model.Uf);
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
            var result = controller.Create(GetNewCondominioModel());

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
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller.Create(GetNewCondominioModel());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult)); // quando inválido, volta pra View
        }

        [TestMethod]
        public void EditTest_Get_Valid()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CondominioViewModel));
            var model = (CondominioViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Condomínio Alfa", model.Nome);
            Assert.AreEqual("Centro", model.Bairro);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(GetTargetCondominioModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CondominioViewModel));
            var model = (CondominioViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Condomínio Alfa", model.Nome);
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

        private static Condominio GetTargetCondominio()
        {
            return new Condominio
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
            };
        }

        private CondominioViewModel GetTargetCondominioModel()
        {
            return new CondominioViewModel
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
            };
        }

        private CondominioViewModel GetNewCondominioModel()
        {
            return new CondominioViewModel
            {
                Id = 99,
                Nome = "Condomínio Novo",
                Cnpj = "99999999999999",
                Rua = "Rua Nova",
                Numero = "100",
                Bairro = "Bairro Novo",
                Cidade = "Cidade Nova",
                Uf = "BA",
                Cep = "87654321",
                Unidades = 20
            };
        }

        private IEnumerable<Condominio> GetTestCondominios()
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
                },
                new Condominio
                {
                    Id = 3,
                    Nome = "Condomínio Gama",
                    Cnpj = "32345678901234",
                    Rua = "Rua C",
                    Numero = "30",
                    Bairro = "Bairro C",
                    Cidade = "Cidade",
                    Uf = "RJ",
                    Cep = "12345678",
                    Unidades = 8
                }
            };
        }
    }
}
