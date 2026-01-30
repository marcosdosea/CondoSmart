using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using CondosmartWeb.Profiles;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class AreaDeLazerControllerTests
    {
        private static AreaDeLazerController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IAreaDeLazerService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AreaDeLazerProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestAreasDeLazer());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetAreaDeLazer());

            mockService.Setup(s => s.Edit(It.IsAny<AreaDeLazer>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<AreaDeLazer>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new AreaDeLazerController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<AreaDeLazerViewModel>));
            var lista = (List<AreaDeLazerViewModel>)viewResult.ViewData.Model;

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AreaDeLazerViewModel));
            var model = (AreaDeLazerViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Piscina", model.Nome);
            Assert.AreEqual("Piscina aquecida com raias para natação", model.Descricao);
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
            var result = controller.Create(GetNewAreaDeLazerModel());

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
            var result = controller.Create(GetNewAreaDeLazerModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AreaDeLazerViewModel));
            var model = (AreaDeLazerViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Piscina", model.Nome);
            Assert.AreEqual("Piscina aquecida com raias para natação", model.Descricao);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(1, GetTargetAreaDeLazerModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void EditTest_Post_Invalid_IdMismatch()
        {
            // Act
            var result = controller.Edit(1, GetTargetAreaDeLazerModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public void DeleteTest_Get_Valid()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AreaDeLazerViewModel));
            var model = (AreaDeLazerViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Piscina", model.Nome);
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

        private static AreaDeLazer GetTargetAreaDeLazer()
        {
            return new AreaDeLazer
            {
                Id = 1,
                Nome = "Piscina",
                Descricao = "Piscina aquecida com raias para natação",
                Disponibilidade = true,
                CondominioId = 1,
                SindicoId = 1,
                CreatedAt = new DateTime(2024, 1, 15)
            };
        }

        private AreaDeLazerViewModel GetTargetAreaDeLazerModel()
        {
            return new AreaDeLazerViewModel
            {
                Id = 1,
                Nome = "Piscina",
                Descricao = "Piscina aquecida com raias para natação",
                Disponibilidade = true,
                CondominioId = 1,
                SindicoId = 1,
                CreatedAt = new DateTime(2024, 1, 15)
            };
        }

        private AreaDeLazerViewModel GetNewAreaDeLazerModel()
        {
            return new AreaDeLazerViewModel
            {
                Id = 99,
                Nome = "Quadra de Esportes",
                Descricao = "Quadra coberta para basquete e vôlei",
                Disponibilidade = true,
                CondominioId = 1,
                SindicoId = 1,
                CreatedAt = new DateTime(2024, 2, 20)
            };
        }

        private List<AreaDeLazer> GetTestAreasDeLazer()
        {
            return new List<AreaDeLazer>
            {
                new AreaDeLazer
                {
                    Id = 1,
                    Nome = "Piscina",
                    Descricao = "Piscina aquecida com raias para natação",
                    Disponibilidade = true,
                    CondominioId = 1,
                    SindicoId = 1,
                    CreatedAt = new DateTime(2024, 1, 15)
                },
                new AreaDeLazer
                {
                    Id = 2,
                    Nome = "Quadra de Esportes",
                    Descricao = "Quadra coberta para basquete e vôlei",
                    Disponibilidade = true,
                    CondominioId = 1,
                    SindicoId = 1,
                    CreatedAt = new DateTime(2024, 1, 20)
                },
                new AreaDeLazer
                {
                    Id = 3,
                    Nome = "Sauna",
                    Descricao = "Sauna seca com capacidade para 10 pessoas",
                    Disponibilidade = false,
                    CondominioId = 2,
                    SindicoId = 2,
                    CreatedAt = new DateTime(2024, 2, 1)
                }
            };
        }
    }
}
