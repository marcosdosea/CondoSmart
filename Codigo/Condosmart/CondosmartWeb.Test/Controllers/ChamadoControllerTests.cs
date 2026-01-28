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
    public class ChamadoControllerTests
    {
        private static ChamadoController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IChamadosService>();
            var mockCondominioService = new Mock<ICondominioService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ChamadoProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestChamados());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetChamado());

            mockService.Setup(s => s.Edit(It.IsAny<Chamado>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Chamado>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            // Mock para validar se o condomínio existe
            mockCondominioService.Setup(s => s.GetById(It.IsAny<int>()))
                .Returns((int id) => id == 1 ? new Condominio { Id = 1, Nome = "Condomínio Teste" } : null);

            controller = new ChamadoController(mockService.Object, mockCondominioService.Object, mapper);
        }

        [TestMethod]
        public void Index_ReturnsView_WithList()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ChamadoViewModel>));
            var lista = (List<ChamadoViewModel>)viewResult.ViewData.Model;

            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void Details_ReturnsView_WithChamado()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ChamadoViewModel));
            var model = (ChamadoViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Vazamento na cozinha", model.Descricao);
            Assert.AreEqual("aberto", model.Status);
        }

        [TestMethod]
        public void Create_Get_ReturnsView()
        {
            var result = controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Post_Valid_RedirectsToIndex()
        {
            var result = controller.Create(GetNewChamadoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Create_Post_Invalid_ReturnsView()
        {
            controller.ModelState.AddModelError("Descricao", "Campo requerido");

            var result = controller.Create(GetNewChamadoModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Edit_Get_ReturnsView_WithChamado()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ChamadoViewModel));
            var model = (ChamadoViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Vazamento na cozinha", model.Descricao);
            Assert.AreEqual("aberto", model.Status);
        }

        [TestMethod]
        public void Edit_Post_Valid_RedirectsToIndex()
        {
            var chamadoModel = GetTargetChamadoModel();
            var result = controller.Edit(chamadoModel.Id, chamadoModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Delete_Get_ReturnsView_WithChamado()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ChamadoViewModel));
            var model = (ChamadoViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Vazamento na cozinha", model.Descricao);
        }

        [TestMethod]
        public void DeleteConfirmed_Post_RedirectsToIndex()
        {
            var result = controller.DeleteConfirmed(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        // ---- Dados de teste ----

        private static Chamado GetTargetChamado()
        {
            return new Chamado
            {
                Id = 1,
                Descricao = "Vazamento na cozinha",
                Status = "aberto",
                MoradorId = 5,
                CondominioId = 1,
                DataChamado = DateTime.Now,
                CreatedAt = DateTime.UtcNow
            };
        }

        private ChamadoViewModel GetTargetChamadoModel()
        {
            return new ChamadoViewModel
            {
                Id = 1,
                Descricao = "Vazamento na cozinha",
                Status = "aberto",
                MoradorId = 5,
                CondominioId = 1,
                DataChamado = DateTime.Now
            };
        }

        private ChamadoViewModel GetNewChamadoModel()
        {
            return new ChamadoViewModel
            {
                Id = 99,
                Descricao = "Luz queimada corredor",
                Status = "aberto",
                MoradorId = 2,
                CondominioId = 1,
                DataChamado = DateTime.Now
            };
        }

        private List<Chamado> GetTestChamados()
        {
            return new List<Chamado>
            {
                new Chamado
                {
                    Id = 1,
                    Descricao = "Vazamento na cozinha",
                    Status = "aberto",
                    MoradorId = 5,
                    CondominioId = 1,
                    DataChamado = DateTime.Now,
                    CreatedAt = DateTime.UtcNow
                },
                new Chamado
                {
                    Id = 2,
                    Descricao = "Porta do salão emperrada",
                    Status = "em_andamento",
                    MoradorId = 3,
                    CondominioId = 1,
                    DataChamado = DateTime.Now,
                    CreatedAt = DateTime.UtcNow
                },
                new Chamado
                {
                    Id = 3,
                    Descricao = "Luz queimada corredor",
                    Status = "resolvido",
                    MoradorId = 2,
                    CondominioId = 1,
                    DataChamado = DateTime.Now,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
