using Microsoft.VisualStudio.TestTools.UnitTesting;
using CondosmartWeb.Controllers;
using CondosmartWeb.Services;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class NotificacoesControllerTests
    {
        private NotificacoesController controller = null!;
        private Mock<INotificacaoService> mockNotificacaoService = null!;
        private Mock<ICondominioContextService> mockCondominioContextService = null!;
        private Mock<IUrlHelper> mockUrlHelper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockNotificacaoService = new Mock<INotificacaoService>();
            mockCondominioContextService = new Mock<ICondominioContextService>();

            controller = new NotificacoesController(mockNotificacaoService.Object, mockCondominioContextService.Object);

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            mockUrlHelper = new Mock<IUrlHelper>();
            controller.Url = mockUrlHelper.Object;
        }

        [TestMethod]
        public void RemoverTest_ReturnUrlLocal_ReturnsRedirect()
        {
            // Arrange
            int notificacaoId = 1;
            string returnUrl = "/local-url";
            mockUrlHelper.Setup(u => u.IsLocalUrl(returnUrl)).Returns(true);

            // Act
            var result = controller.Remover(notificacaoId, returnUrl);

            // Assert
            mockNotificacaoService.Verify(s => s.Remover(notificacaoId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(returnUrl, ((RedirectResult)result).Url);
            Assert.AreEqual("Notificacao removida.", controller.TempData["Sucesso"]);
        }

        [TestMethod]
        public void RemoverTest_ReturnUrlNotLocal_ReturnsRedirectToHome()
        {
            // Arrange
            int notificacaoId = 2;
            string returnUrl = "http://external-url.com";
            mockUrlHelper.Setup(u => u.IsLocalUrl(returnUrl)).Returns(false);

            // Act
            var result = controller.Remover(notificacaoId, returnUrl);

            // Assert
            mockNotificacaoService.Verify(s => s.Remover(notificacaoId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        [TestMethod]
        public void LimparTodasTest_RemovesAndRedirects()
        {
            // Arrange
            int condominioId = 5;
            string returnUrl = "/dashboard";
            mockCondominioContextService.Setup(s => s.GetCondominioAtualId()).Returns(condominioId);
            mockUrlHelper.Setup(u => u.IsLocalUrl(returnUrl)).Returns(true);

            // Act
            var result = controller.LimparTodas(returnUrl);

            // Assert
            mockCondominioContextService.Verify(s => s.GetCondominioAtualId(), Times.Once);
            mockNotificacaoService.Verify(s => s.LimparPorCondominio(condominioId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(returnUrl, ((RedirectResult)result).Url);
            Assert.AreEqual("Notificacoes removidas com sucesso.", controller.TempData["Sucesso"]);
        }
    }
}
