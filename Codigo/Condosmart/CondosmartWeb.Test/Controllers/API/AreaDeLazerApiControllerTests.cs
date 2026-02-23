using AutoMapper;
using CondosmartAPI.Mappers;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

// Aliases para resolver conflito de nomes com o AreaDeLazerController do MVC
using ApiController = CondosmartAPI.Controllers.AreaDeLazerController;
using ApiViewModel = CondosmartAPI.Models.AreaDeLazerViewModel;

namespace CondosmartAPI.Controllers.Tests
{
    [TestClass]
    public class AreaDeLazerApiControllerTests
    {
        private static ApiController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IAreaDeLazerService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AreaDeLazerProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestAreas());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetArea());

            mockService.Setup(s => s.GetById(99))
                .Returns((AreaDeLazer?)null);

            mockService.Setup(s => s.Create(It.IsAny<AreaDeLazer>()))
                .Returns(10);

            mockService.Setup(s => s.Edit(It.IsAny<AreaDeLazer>()))
                .Verifiable();

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new ApiController(mockService.Object, mapper);
        }

        // ---- GET /api/areadelazer ----

        [TestMethod]
        public void GetAll_Valido_Retorna200ComLista()
        {
            var result = controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<ApiViewModel>));
            var lista = (List<ApiViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // ---- GET /api/areadelazer/{id} ----

        [TestMethod]
        public void GetById_Valido_Retorna200ComArea()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(ApiViewModel));
            var model = (ApiViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Piscina", model.Nome);
            Assert.AreEqual(1, model.CondominioId);
            Assert.IsTrue(model.Disponibilidade);
        }

        [TestMethod]
        public void GetById_NaoEncontrado_Retorna404()
        {
            var result = controller.GetById(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- POST /api/areadelazer ----

        [TestMethod]
        public void Create_Valido_Retorna201Created()
        {
            var result = controller.Create(GetNewAreaModel());

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public void Create_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewAreaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- PUT /api/areadelazer/{id} ----

        [TestMethod]
        public void Edit_Valido_Retorna200ComArea()
        {
            var vm = GetTargetAreaModel();

            var result = controller.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(ApiViewModel));
            var model = (ApiViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Piscina", model.Nome);
            Assert.AreEqual(1, model.CondominioId);
        }

        [TestMethod]
        public void Edit_IdDivergente_Retorna400()
        {
            var result = controller.Edit(99, GetTargetAreaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Edit_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Edit(1, GetTargetAreaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Edit_NaoEncontrado_Retorna404()
        {
            var vm = GetTargetAreaModel();
            vm.Id = 99;

            var result = controller.Edit(99, vm);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- DELETE /api/areadelazer/{id} ----

        [TestMethod]
        public void Delete_Valido_Retorna200()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Delete_NaoEncontrado_Retorna404()
        {
            var result = controller.Delete(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // --------- Dados de Teste ---------

        private static AreaDeLazer GetTargetArea() => new()
        {
            Id = 1,
            Nome = "Piscina",
            Descricao = "Piscina olímpica",
            CondominioId = 1,
            SindicoId = 1,
            Disponibilidade = true,
            CreatedAt = new DateTime(2024, 1, 15)
        };

        private static ApiViewModel GetTargetAreaModel() => new()
        {
            Id = 1,
            Nome = "Piscina",
            Descricao = "Piscina olímpica",
            CondominioId = 1,
            SindicoId = 1,
            Disponibilidade = true
        };

        private static ApiViewModel GetNewAreaModel() => new()
        {
            Nome = "Churrasqueira",
            Descricao = "Churrasqueira coberta",
            CondominioId = 1,
            Disponibilidade = true
        };

        private static List<AreaDeLazer> GetTestAreas() => new()
        {
            GetTargetArea(),
            new AreaDeLazer { Id = 2, Nome = "Churrasqueira", CondominioId = 1, Disponibilidade = true },
            new AreaDeLazer { Id = 3, Nome = "Quadra",        CondominioId = 1, Disponibilidade = false }
        };
    }
}
