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
    public class VisitanteControllerTests
    {
        private static VisitanteController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IVisitanteService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new VisitanteProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestVisitantes());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetVisitante());

            mockService.Setup(s => s.Edit(It.IsAny<Visitantes>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Visitantes>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new VisitanteController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<VisitanteViewModel>));
            var lista = (List<VisitanteViewModel>)viewResult.ViewData.Model;

            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Detail(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VisitanteViewModel));
            var model = (VisitanteViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("João Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
            Assert.AreEqual("11987654321", model.Telefone);
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
            var result = controller.Create(GetNewVisitanteModel());

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
            var result = controller.Create(GetNewVisitanteModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VisitanteViewModel));
            var model = (VisitanteViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("João Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(1, GetTargetVisitanteModel());

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

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VisitanteViewModel));
            var model = (VisitanteViewModel)viewResult.ViewData.Model;

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

        private static Visitantes GetTargetVisitante()
        {
            return new Visitantes
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Telefone = "11987654321",
                Observacao = "Visitante frequente",
                DataHoraEntrada = new DateTime(2024, 1, 15, 10, 30, 0),
                DataHoraSaida = null
            };
        }

        private VisitanteViewModel GetTargetVisitanteModel()
        {
            return new VisitanteViewModel
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Telefone = "11987654321",
                Observacao = "Visitante frequente",
                DataHoraEntrada = new DateTime(2024, 1, 15, 10, 30, 0),
                DataHoraSaida = null
            };
        }

        private VisitanteViewModel GetNewVisitanteModel()
        {
            return new VisitanteViewModel
            {
                Id = 99,
                Nome = "Maria Santos",
                Cpf = "98765432109",
                Telefone = "11912345678",
                Observacao = "Primeira visita",
                DataHoraEntrada = new DateTime(2024, 1, 20, 14, 0, 0),
                DataHoraSaida = null
            };
        }

        private List<Visitantes> GetTestVisitantes()
        {
            return new List<Visitantes>
            {
                new Visitantes
                {
                    Id = 1,
                    Nome = "João Silva",
                    Cpf = "12345678901",
                    Telefone = "11987654321",
                    Observacao = "Visitante frequente",
                    DataHoraEntrada = new DateTime(2024, 1, 15, 10, 30, 0),
                    DataHoraSaida = null
                },
                new Visitantes
                {
                    Id = 2,
                    Nome = "Maria Oliveira",
                    Cpf = "23456789012",
                    Telefone = "11976543210",
                    Observacao = "Entrega de encomenda",
                    DataHoraEntrada = new DateTime(2024, 1, 16, 14, 0, 0),
                    DataHoraSaida = new DateTime(2024, 1, 16, 14, 30, 0)
                },
                new Visitantes
                {
                    Id = 3,
                    Nome = "Carlos Souza",
                    Cpf = "34567890123",
                    Telefone = "11965432109",
                    Observacao = "Técnico de manutenção",
                    DataHoraEntrada = new DateTime(2024, 1, 17, 9, 0, 0),
                    DataHoraSaida = new DateTime(2024, 1, 17, 12, 0, 0)
                }
            };
        }
    }
}
