using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Mappers;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MoradorControllerTests
    {
        private static MoradorController controller = null!;

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

            mockService.Setup(s => s.Edit(It.IsAny<Morador>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Morador>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new MoradorController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<MoradorViewModel>));
            var lista = (List<MoradorViewModel>)viewResult.ViewData.Model;

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
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
            var result = controller.Create(GetNewMoradorModel());

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
            var result = controller.Create(GetNewMoradorModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(GetTargetMoradorModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Maria Silva", model.Nome);
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
                Cidade = "São Paulo",
                Uf = "SP",
                CondominioId = 1
            };
        }

        private MoradorViewModel GetNewMoradorModel()
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

        private List<Morador> GetTestMoradores()
        {
            return new List<Morador>
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
}
