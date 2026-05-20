using Microsoft.VisualStudio.TestTools.UnitTesting;
using Condosmart.Controllers;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Condosmart.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController controller = null!;
        private Mock<ILogger<HomeController>> mockLogger = null!;
        private Mock<IAdminDashboardService> mockDashboardService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<HomeController>>();
            mockDashboardService = new Mock<IAdminDashboardService>();

            controller = new HomeController(mockLogger.Object, mockDashboardService.Object);

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        }

        [TestMethod]
        public void Index_RetornaViewComDashboard()
        {
            // Arrange
            var dashboard = new DashboardViewModel { TotalUnidades = 10 };
            mockDashboardService.Setup(s => s.Build()).Returns(dashboard);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dashboard, result.Model);
        }

        [TestMethod]
        public void Privacy_RetornaView()
        {
            // Act
            var result = controller.Privacy() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Error_RetornaViewComRequestId()
        {
            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.IsFalse(string.IsNullOrEmpty(model.RequestId));
        }
    }
}
