using Microsoft.VisualStudio.TestTools.UnitTesting;
using CondosmartWeb.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class CepControllerTests
    {
        private CepController controller = null!;
        private Mock<IHttpClientFactory> mockHttpClientFactory = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            controller = new CepController(mockHttpClientFactory.Object);

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        }

        [TestMethod]
        public async Task GetEndereco_CepVazio_RetornaBadRequest()
        {
            // Act
            var result = await controller.GetEndereco("");

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetEndereco_CepInvalido_RetornaBadRequest()
        {
            // Act
            var result = await controller.GetEndereco("123");

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetEndereco_CepValido_RetornaJson()
        {
            // Arrange
            var json = "{\"logradouro\":\"Praca da Se\",\"complemento\":\"\",\"bairro\":\"Se\",\"localidade\":\"Sao Paulo\",\"uf\":\"SP\"}";
            var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
            var client = new HttpClient(handler);
            mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var result = await controller.GetEndereco("01001000") as JsonResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }

    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_response)
            });
        }
    }
}
