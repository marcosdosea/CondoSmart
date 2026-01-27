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
    public class SindicoControllerTests
    {
        private static SindicoController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<ISindicoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new SindicoProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestSindicos());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetSindico());

            mockService.Setup(s => s.Edit(It.IsAny<Sindico>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Sindico>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new SindicoController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<SindicoViewModel>));
            var lista = (List<SindicoViewModel>)viewResult.ViewData.Model;

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(SindicoViewModel));
            var model = (SindicoViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("João Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
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
            var result = controller.Create(GetNewSindicoModel());

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
            var result = controller.Create(GetNewSindicoModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(SindicoViewModel));
            var model = (SindicoViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("João Silva", model.Nome);
            Assert.AreEqual("Centro", model.Bairro);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(GetTargetSindicoModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(SindicoViewModel));
            var model = (SindicoViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("João Silva", model.Nome);
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

        private static Sindico GetTargetSindico()
        {
            return new Sindico
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Rua = "Rua A",
                Numero = "10",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP",
                Cep = "12345678"
            };
        }

        private SindicoViewModel GetTargetSindicoModel()
        {
            return new SindicoViewModel
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Rua = "Rua A",
                Numero = "10",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP",
                Cep = "12345678"
            };
        }

        private SindicoViewModel GetNewSindicoModel()
        {
            return new SindicoViewModel
            {
                Id = 99,
                Nome = "Maria Santos",
                Cpf = "98765432101",
                Rua = "Rua Nova",
                Numero = "100",
                Bairro = "Bairro Novo",
                Cidade = "Rio de Janeiro",
                Uf = "RJ",
                Cep = "87654321"
            };
        }

        private List<Sindico> GetTestSindicos()
        {
            return new List<Sindico>
            {
                new Sindico
                {
                    Id = 1,
                    Nome = "João Silva",
                    Cpf = "12345678901",
                    Rua = "Rua A",
                    Numero = "10",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Uf = "SP",
                    Cep = "12345678"
                },
                new Sindico
                {
                    Id = 2,
                    Nome = "Maria Santos",
                    Cpf = "22345678901",
                    Rua = "Rua B",
                    Numero = "20",
                    Bairro = "Vila Mariana",
                    Cidade = "São Paulo",
                    Uf = "SP",
                    Cep = "12345678"
                },
                new Sindico
                {
                    Id = 3,
                    Nome = "Carlos Oliveira",
                    Cpf = "32345678901",
                    Rua = "Rua C",
                    Numero = "30",
                    Bairro = "Centro",
                    Cidade = "Rio de Janeiro",
                    Uf = "RJ",
                    Cep = "12345678"
                }
            };
        }
    }
}
