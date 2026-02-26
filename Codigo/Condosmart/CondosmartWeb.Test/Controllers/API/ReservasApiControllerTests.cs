using AutoMapper;
using CondosmartAPI.Controllers;
using CondosmartAPI.Mappers;
using CondosmartAPI.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class ReservasApiControllerTests
    {
        private static ReservasController controller = null!;
        private static Mock<IReservaService> mockService = null!;
        private static IMapper mapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IReservaService>();

            mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ReservaProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestReservas());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetReserva());

            mockService.Setup(s => s.GetById(99))
                .Returns((Reserva?)null);

            mockService.Setup(s => s.Create(It.IsAny<Reserva>()))
                .Returns(10);

            mockService.Setup(s => s.Edit(It.IsAny<Reserva>()))
                .Verifiable();

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new ReservasController(mockService.Object, mapper);
        }

        // ---- GET /api/reservas ----

        [TestMethod]
        public void GetAll_Valido_Retorna200ComLista()
        {
            var result = controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<ReservaViewModel>));
            var lista = (List<ReservaViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // ---- GET /api/reservas/{id} ----

        [TestMethod]
        public void GetById_Valido_Retorna200ComReserva()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(ReservaViewModel));
            var model = (ReservaViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(2, model.AreaId);
            Assert.AreEqual(1, model.CondominioId);
            Assert.AreEqual("confirmado", model.Status);
            Assert.AreEqual("Piscina", model.NomeArea);
            Assert.AreEqual("João Silva", model.NomeMorador);
        }

        [TestMethod]
        public void GetById_NaoEncontrado_Retorna404()
        {
            var result = controller.GetById(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- POST /api/reservas ----

        [TestMethod]
        public void Create_Valido_Retorna201Created()
        {
            var result = controller.Create(GetNewReservaModel());

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public void Create_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("AreaId", "Campo requerido");

            var result = controller.Create(GetNewReservaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Create_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<IReservaService>();
            mockLocal.Setup(s => s.Create(It.IsAny<Reserva>()))
                .Throws(new ArgumentException("A data/hora de fim não pode ser anterior à data/hora de início."));

            var controllerLocal = new ReservasController(mockLocal.Object, mapper);
            var vm = GetNewReservaModel();
            vm.DataFim = vm.DataInicio.AddHours(-1);

            var result = controllerLocal.Create(vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- PUT /api/reservas/{id} ----

        [TestMethod]
        public void Edit_Valido_Retorna200ComReserva()
        {
            var vm = GetTargetReservaModel();

            var result = controller.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(ReservaViewModel));
            var model = (ReservaViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("confirmado", model.Status);
        }

        [TestMethod]
        public void Edit_IdDivergente_Retorna400()
        {
            var result = controller.Edit(99, GetTargetReservaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Edit_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Status", "Campo requerido");

            var result = controller.Edit(1, GetTargetReservaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Edit_NaoEncontrado_Retorna404()
        {
            var vm = GetTargetReservaModel();
            vm.Id = 99;

            var result = controller.Edit(99, vm);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Edit_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<IReservaService>();
            mockLocal.Setup(s => s.GetById(1)).Returns(GetTargetReserva());
            mockLocal.Setup(s => s.Edit(It.IsAny<Reserva>()))
                .Throws(new ArgumentException("Status inválido."));

            var controllerLocal = new ReservasController(mockLocal.Object, mapper);
            var vm = GetTargetReservaModel();
            vm.Status = "invalido";

            var result = controllerLocal.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- DELETE /api/reservas/{id} ----

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

        private static Reserva GetTargetReserva() => new()
        {
            Id = 1,
            AreaId = 2,
            CondominioId = 1,
            MoradorId = 5,
            DataInicio = new DateTime(2026, 1, 1, 10, 0, 0),
            DataFim = new DateTime(2026, 1, 1, 12, 0, 0),
            Status = "confirmado",
            CreatedAt = new DateTime(2025, 12, 1),
            Area = new AreaDeLazer { Id = 2, Nome = "Piscina", CondominioId = 1 },
            Morador = new Morador { Id = 5, Nome = "João Silva", Cpf = "11122233344", CondominioId = 1 }
        };

        private static ReservaViewModel GetTargetReservaModel() => new()
        {
            Id = 1,
            AreaId = 2,
            CondominioId = 1,
            MoradorId = 5,
            DataInicio = new DateTime(2026, 1, 1, 10, 0, 0),
            DataFim = new DateTime(2026, 1, 1, 12, 0, 0),
            Status = "confirmado"
        };

        private static ReservaViewModel GetNewReservaModel() => new()
        {
            AreaId = 3,
            CondominioId = 1,
            MoradorId = 6,
            DataInicio = DateTime.UtcNow.AddDays(1),
            DataFim = DateTime.UtcNow.AddDays(1).AddHours(2),
            Status = "pendente"
        };

        private static List<Reserva> GetTestReservas() => new()
        {
            GetTargetReserva(),
            new Reserva
            {
                Id = 2, AreaId = 3, CondominioId = 1, MoradorId = 4,
                DataInicio = DateTime.UtcNow.AddDays(2),
                DataFim = DateTime.UtcNow.AddDays(2).AddHours(3),
                Status = "pendente", CreatedAt = DateTime.UtcNow,
                Area = new AreaDeLazer { Id = 3, Nome = "Churrasqueira", CondominioId = 1 },
                Morador = new Morador { Id = 4, Nome = "Ana Costa", Cpf = "55566677788", CondominioId = 1 }
            },
            new Reserva
            {
                Id = 3, AreaId = 4, CondominioId = 1, MoradorId = 2,
                DataInicio = DateTime.UtcNow.AddDays(3),
                DataFim = DateTime.UtcNow.AddDays(3).AddHours(1),
                Status = "cancelado", CreatedAt = DateTime.UtcNow,
                Area = new AreaDeLazer { Id = 4, Nome = "Quadra", CondominioId = 1 },
                Morador = new Morador { Id = 2, Nome = "Carlos Mota", Cpf = "99988877766", CondominioId = 1 }
            }
        };
    }
}
