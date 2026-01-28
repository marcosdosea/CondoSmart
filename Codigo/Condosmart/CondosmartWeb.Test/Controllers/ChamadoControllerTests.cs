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

            controller = new ChamadoController(mockService.Object, mapper);
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
            Assert.AreEqual("Alta", model.Prioridade);
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
            Assert.AreEqual("Cozinha 101", model.Local);
        }

        [TestMethod]
        public void Edit_Post_Valid_RedirectsToIndex()
        {
            var result = controller.Edit(GetTargetChamadoModel());

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
                Descricao = "Vazamento no encanamento da pia",
                Prioridade = "Alta",
                Local = "Cozinha 101",
                MoradorId = 5,
                CreatedAt = DateTime.UtcNow
            };
        }

        private ChamadoViewModel GetTargetChamadoModel()
        {
            return new ChamadoViewModel
            {
                Id = 1,
                Descricao = "Vazamento na cozinha",
                Descricao = "Vazamento no encanamento da pia",
                Prioridade = "Alta",
                Local = "Cozinha 101",
                MoradorId = 5
            };
        }

        private ChamadoViewModel GetNewChamadoModel()
        {
            return new ChamadoViewModel
            {
                Id = 99,
                Descricao = "Luz queimada corredor",
                Descricao = "Lâmpada queimada no corredor principal",
                Prioridade = "Média",
                Local = "Corredor térreo",
                MoradorId = 2
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
                    Descricao = "Vazamento no encanamento da pia",
                    Prioridade = "Alta",
                    Local = "Cozinha 101",
                    MoradorId = 5,
                    CreatedAt = DateTime.UtcNow
                },
                new Chamado
                {
                    Id = 2,
                    Descricao = "Porta do salão emperrada",
                    Descricao = "Porta não fecha",
                    Prioridade = "Baixa",
                    Local = "Salão de festas",
                    MoradorId = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Chamado
                {
                    Id = 3,
                    Descricao = "Luz queimada corredor",
                    Descricao = "Lâmpada queimada no corredor principal",
                    Prioridade = "Média",
                    Local = "Corredor térreo",
                    MoradorId = 2,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
