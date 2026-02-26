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
    public class AtasApiControllerTests
    {
        private static AtasController controller = null!;
        private static Mock<IAtaService> mockService = null!;
        private static IMapper mapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IAtaService>();

            mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AtaProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestAtas());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetAta());

            mockService.Setup(s => s.GetById(99))
                .Returns((Ata?)null);

            mockService.Setup(s => s.Create(It.IsAny<Ata>()))
                .Returns(10);

            mockService.Setup(s => s.Edit(It.IsAny<Ata>()))
                .Verifiable();

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new AtasController(mockService.Object, mapper);
        }

        // ---- GET /api/atas ----

        [TestMethod]
        public void GetAll_Valido_Retorna200ComLista()
        {
            var result = controller.GetAll();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(List<AtaViewModel>));
            var lista = (List<AtaViewModel>)ok.Value!;

            Assert.HasCount(3, lista);
        }

        // ---- GET /api/atas/{id} ----

        [TestMethod]
        public void GetById_Valido_Retorna200ComAta()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(AtaViewModel));
            var model = (AtaViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Assembléia Ordinária", model.Titulo);
            Assert.AreEqual("Manutenção, Obras", model.Temas);
            Assert.AreEqual(1, model.CondominioId);
            Assert.AreEqual("Condomínio Beija Flor", model.NomeCondominio);
            Assert.AreEqual("João Silva", model.NomeSindico);
        }

        [TestMethod]
        public void GetById_NaoEncontrado_Retorna404()
        {
            var result = controller.GetById(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ---- POST /api/atas ----

        [TestMethod]
        public void Create_Valido_Retorna201Created()
        {
            var result = controller.Create(GetNewAtaModel());

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var created = (CreatedAtActionResult)result.Result!;

            Assert.AreEqual(201, created.StatusCode);
            Assert.AreEqual(nameof(controller.GetById), created.ActionName);
        }

        [TestMethod]
        public void Create_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Titulo", "Campo requerido");

            var result = controller.Create(GetNewAtaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Create_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<IAtaService>();
            mockLocal.Setup(s => s.Create(It.IsAny<Ata>()))
                .Throws(new ArgumentException("O título da ata é obrigatório."));

            var controllerLocal = new AtasController(mockLocal.Object, mapper);
            var vm = GetNewAtaModel();
            vm.Titulo = string.Empty;

            var result = controllerLocal.Create(vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- PUT /api/atas/{id} ----

        [TestMethod]
        public void Edit_Valido_Retorna200ComAta()
        {
            var vm = GetTargetAtaModel();

            var result = controller.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result.Result!;

            Assert.IsInstanceOfType(ok.Value, typeof(AtaViewModel));
            var model = (AtaViewModel)ok.Value!;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Assembléia Ordinária", model.Titulo);
        }

        [TestMethod]
        public void Edit_IdDivergente_Retorna400()
        {
            var result = controller.Edit(99, GetTargetAtaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Edit_ModeloInvalido_Retorna400()
        {
            controller.ModelState.AddModelError("Conteudo", "Campo requerido");

            var result = controller.Edit(1, GetTargetAtaModel());

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Edit_NaoEncontrado_Retorna404()
        {
            var vm = GetTargetAtaModel();
            vm.Id = 99;

            var result = controller.Edit(99, vm);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Edit_ServicoLancaArgumentException_Retorna400()
        {
            var mockLocal = new Mock<IAtaService>();
            mockLocal.Setup(s => s.GetById(1)).Returns(GetTargetAta());
            mockLocal.Setup(s => s.Edit(It.IsAny<Ata>()))
                .Throws(new ArgumentException("Condomínio inválido."));

            var controllerLocal = new AtasController(mockLocal.Object, mapper);
            var vm = GetTargetAtaModel();
            vm.CondominioId = 0;

            var result = controllerLocal.Edit(vm.Id, vm);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ---- DELETE /api/atas/{id} ----

        [TestMethod]
        public void Delete_Valido_Retorna204NoContent()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void Delete_NaoEncontrado_Retorna404()
        {
            var result = controller.Delete(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // --------- Dados de Teste ---------

        private static Ata GetTargetAta() => new()
        {
            Id = 1,
            Titulo = "Assembléia Ordinária",
            Temas = "Manutenção, Obras",
            Conteudo = "Conteúdo da assembleia ordinária...",
            DataReuniao = new DateOnly(2026, 1, 15),
            CondominioId = 1,
            SindicoId = 1,
            CreatedAt = new DateTime(2025, 12, 1),
            Condominio = new Condominio { Id = 1, Nome = "Condomínio Beija Flor", Cnpj = "12345678901234" },
            Sindico = new Sindico { Id = 1, Nome = "João Silva", Cpf = "11122233344" }
        };

        private static AtaViewModel GetTargetAtaModel() => new()
        {
            Id = 1,
            Titulo = "Assembléia Ordinária",
            Temas = "Manutenção, Obras",
            Conteudo = "Conteúdo da assembleia ordinária...",
            DataReuniao = new DateTime(2026, 1, 15),
            CondominioId = 1,
            SindicoId = 1
        };

        private static AtaViewModel GetNewAtaModel() => new()
        {
            Titulo = "Assembléia Extraordinária",
            Temas = "Eleição de Síndico",
            Conteudo = "Conteúdo da assembleia extraordinária...",
            DataReuniao = DateTime.UtcNow.AddDays(30),
            CondominioId = 1,
            SindicoId = 1
        };

        private static List<Ata> GetTestAtas() => new()
        {
            GetTargetAta(),
            new Ata
            {
                Id = 2,
                Titulo = "Assembléia Extraordinária",
                Temas = "Eleição de Síndico",
                Conteudo = "Conteúdo da assembleia extraordinária...",
                DataReuniao = new DateOnly(2026, 2, 20),
                CondominioId = 1,
                SindicoId = 1,
                CreatedAt = DateTime.UtcNow,
                Condominio = new Condominio { Id = 1, Nome = "Condomínio Beija Flor", Cnpj = "12345678901234" },
                Sindico = new Sindico { Id = 1, Nome = "João Silva", Cpf = "11122233344" }
            },
            new Ata
            {
                Id = 3,
                Titulo = "Assembleia Geral",
                Temas = "Aprovação de Contas",
                Conteudo = "Conteúdo da assembleia geral...",
                DataReuniao = new DateOnly(2026, 3, 10),
                CondominioId = 2,
                SindicoId = 2,
                CreatedAt = DateTime.UtcNow,
                Condominio = new Condominio { Id = 2, Nome = "Condomínio Primavera", Cnpj = "98765432109876" },
                Sindico = new Sindico { Id = 2, Nome = "Maria Santos", Cpf = "55566677788" }
            }
        };
    }
}
