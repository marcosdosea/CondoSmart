using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Mappers;
using CondosmartWeb.Models;
using CondosmartWeb.Profiles;
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
    public class ReservaControllerTests
    {
        private static ReservaController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IReservaService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockAreaService = new Mock<IAreaDeLazerService>();
            var mockMoradorService = new Mock<IMoradorService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ReservaProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestReservas());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetReserva());

            mockService.Setup(s => s.Edit(It.IsAny<Reserva>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Reserva>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio>());
            mockAreaService.Setup(s => s.GetAll()).Returns(new List<AreaDeLazer>());
            mockMoradorService.Setup(s => s.GetAll()).Returns(new List<Morador>());

            controller = new ReservaController(mockService.Object, mockCondominioService.Object, mockAreaService.Object, mockMoradorService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ReservaViewModel>));
            var lista = (List<ReservaViewModel>)viewResult.ViewData.Model;

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ReservaViewModel));
            var model = (ReservaViewModel)viewResult.ViewData.Model;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(2, model.AreaId);
            Assert.AreEqual(1, model.CondominioId);
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
            var result = controller.Create(GetNewReservaModel());

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
            controller.ModelState.AddModelError("AreaId", "Campo requerido");

            // Act
            var result = controller.Create(GetNewReservaModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ReservaViewModel));
            var model = (ReservaViewModel)viewResult.ViewData.Model;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(2, model.AreaId);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var model = GetTargetReservaModel();
            var result = controller.Edit(model.Id, model);

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ReservaViewModel));
            var model = (ReservaViewModel)viewResult.ViewData.Model;

            Assert.AreEqual(1, model.Id);
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

        private static Reserva GetTargetReserva()
        {
            return new Reserva
            {
                Id = 1,
                AreaId = 2,
                CondominioId = 1,
                DataInicio = new DateTime(2026, 01, 01, 10, 0, 0),
                DataFim = new DateTime(2026, 01, 01, 12, 0, 0),
                MoradorId = 5,
                Status = "Confirmada",
                CreatedAt = DateTime.UtcNow
            };
        }

        private ReservaViewModel GetTargetReservaModel()
        {
            return new ReservaViewModel
            {
                Id = 1,
                AreaId = 2,
                CondominioId = 1,
                DataInicio = new DateTime(2026, 01, 01, 10, 0, 0),
                DataFim = new DateTime(2026, 01, 01, 12, 0, 0),
                MoradorId = 5,
                Status = "Confirmada"
            };
        }

        private ReservaViewModel GetNewReservaModel()
        {
            return new ReservaViewModel
            {
                Id = 99,
                AreaId = 3,
                CondominioId = 1,
                DataInicio = DateTime.UtcNow.AddDays(1),
                DataFim = DateTime.UtcNow.AddDays(1).AddHours(2),
                MoradorId = 6,
                Status = "Pendente"
            };
        }

        private List<Reserva> GetTestReservas()
        {
            return new List<Reserva>
                    {
                        new Reserva
                        {
                            Id = 1,
                            AreaId = 2,
                            CondominioId = 1,
                            DataInicio = DateTime.UtcNow.AddDays(1),
                            DataFim = DateTime.UtcNow.AddDays(1).AddHours(2),
                            MoradorId = 5,
                            Status = "Confirmada",
                            CreatedAt = DateTime.UtcNow
                        },
                        new Reserva
                        {
                            Id = 2,
                            AreaId = 3,
                            CondominioId = 1,
                            DataInicio = DateTime.UtcNow.AddDays(2),
                            DataFim = DateTime.UtcNow.AddDays(2).AddHours(3),
                            MoradorId = 4,
                            Status = "Pendente",
                            CreatedAt = DateTime.UtcNow
                        },
                        new Reserva
                        {
                            Id = 3,
                            AreaId = 4,
                            CondominioId = 1,
                            DataInicio = DateTime.UtcNow.AddDays(3),
                            DataFim = DateTime.UtcNow.AddDays(3).AddHours(1),
                            MoradorId = 2,
                            Status = "Cancelada",
                            CreatedAt = DateTime.UtcNow
                        }
                    };
        }
    }
}
