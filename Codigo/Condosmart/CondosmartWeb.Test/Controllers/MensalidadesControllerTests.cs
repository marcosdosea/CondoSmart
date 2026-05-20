using Microsoft.VisualStudio.TestTools.UnitTesting;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Models;
using Core.Service;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MensalidadesControllerTests
    {
        private MensalidadesController controller = null!;
        private Mock<IMensalidadeService> mockMensalidadeService = null!;
        private Mock<ICondominioService> mockCondominioService = null!;
        private Mock<IUnidadesResidenciaisService> mockUnidadesService = null!;
        private Mock<ICondominioContextService> mockCondominioContextService = null!;
        private Mock<INotificacaoService> mockNotificacaoService = null!;
        private Mock<IMapper> mockMapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockMensalidadeService = new Mock<IMensalidadeService>();
            mockCondominioService = new Mock<ICondominioService>();
            mockUnidadesService = new Mock<IUnidadesResidenciaisService>();
            mockCondominioContextService = new Mock<ICondominioContextService>();
            mockNotificacaoService = new Mock<INotificacaoService>();
            mockMapper = new Mock<IMapper>();

            controller = new MensalidadesController(
                mockMensalidadeService.Object,
                mockCondominioService.Object,
                mockUnidadesService.Object,
                mockCondominioContextService.Object,
                mockNotificacaoService.Object,
                mockMapper.Object);

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            controller.Url = new Mock<IUrlHelper>().Object;
        }

        [TestMethod]
        public void Details_IdExistente_RetornaView()
        {
            // Arrange
            var mensalidade = new Mensalidade { Id = 1 };
            var viewModel = new MensalidadeViewModel { Id = 1 };
            mockMensalidadeService.Setup(s => s.GetById(1)).Returns(mensalidade);
            mockMapper.Setup(m => m.Map<MensalidadeViewModel>(mensalidade)).Returns(viewModel);

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewModel, result.Model);
        }

        [TestMethod]
        public void Details_IdInexistente_RetornaNotFound()
        {
            // Arrange
            mockMensalidadeService.Setup(s => s.GetById(99)).Returns((Mensalidade?)null);

            // Act
            var result = controller.Details(99);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Comprovante_RedirecionaParaDetails()
        {
            // Act
            var result = controller.Comprovante(5) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual("Comprovante indisponivel enquanto o fluxo de pagamento nao for implementado.", controller.TempData["Erro"]);
        }
    }
}
