using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class AtaControllerTests
    {
        private AtaController _controller = null!;
        private Mock<IHttpClientFactory> _mockHttpClientFactory = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _controller = new AtaController(_mockHttpClientFactory.Object);
        }

        [TestMethod]
        public void ControllerInstanciado_Valido()
        {
            // Assert
            Assert.IsNotNull(_controller);
            Assert.IsInstanceOfType(_controller, typeof(AtaController));
        }

        [TestMethod]
        public void CreateClient_Utilizado_Corretamente()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7070/")
            };

            _mockHttpClientFactory
                .Setup(_ => _.CreateClient("CondoSmartAPI"))
                .Returns(httpClient);

            // Act
            var client = _mockHttpClientFactory.Object.CreateClient("CondoSmartAPI");

            // Assert
            Assert.IsNotNull(client);
            _mockHttpClientFactory.Verify(_ => _.CreateClient("CondoSmartAPI"), Times.Once);
        }
    }
}
