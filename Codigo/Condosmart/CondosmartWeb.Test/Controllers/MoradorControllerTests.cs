using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MoradorControllerTests
    {
        private static MoradorController controller = null!;
        private static Mock<HttpMessageHandler> mockHandler = null!;
        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        [TestInitialize]
        public void Initialize()
        {
            mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7290/")
            };

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient("CondosmartApi")).Returns(httpClient);

            controller = new MoradorController(mockFactory.Object);
        }

        private void SetupHandler(HttpMethod method, HttpStatusCode statusCode, object? content = null, string? urlContains = null)
        {
            var response = new HttpResponseMessage(statusCode);
            if (content != null)
                response.Content = new StringContent(JsonSerializer.Serialize(content), System.Text.Encoding.UTF8, "application/json");

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == method),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        [TestMethod]
        public async Task IndexTest_Valido()
        {
            SetupHandler(HttpMethod.Get, HttpStatusCode.OK, GetTestMoradores());

            var result = await controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<MoradorViewModel>));
            var lista = (List<MoradorViewModel>)viewResult.ViewData.Model;
            Assert.HasCount(3, lista);
        }

        [TestMethod]
        public async Task DetailsTest_Valido()
        {
            SetupHandler(HttpMethod.Get, HttpStatusCode.OK, GetTargetMoradorModel());

            var result = await controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            SetupHandler(HttpMethod.Post, HttpStatusCode.Created, GetTargetMoradorModel());

            var result = await controller.Create(GetNewMoradorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = await controller.Create(GetNewMoradorModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task EditTest_Get_Valid()
        {
            SetupHandler(HttpMethod.Get, HttpStatusCode.OK, GetTargetMoradorModel());

            var result = await controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual("Maria Silva", model.Nome);
            Assert.AreEqual("12345678901", model.Cpf);
        }

        [TestMethod]
        public async Task EditTest_Post_Valid()
        {
            SetupHandler(HttpMethod.Put, HttpStatusCode.OK, GetTargetMoradorModel());

            var result = await controller.Edit(GetTargetMoradorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task DeleteTest_Get_Valid()
        {
            SetupHandler(HttpMethod.Get, HttpStatusCode.OK, GetTargetMoradorModel());

            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MoradorViewModel));
            var model = (MoradorViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual("Maria Silva", model.Nome);
        }

        [TestMethod]
        public async Task DeleteTest_Post_Valid()
        {
            SetupHandler(HttpMethod.Delete, HttpStatusCode.OK);

            var result = await controller.DeleteConfirmed(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        // --------- Dados de Teste ---------

        private static MoradorViewModel GetTargetMoradorModel()
        {
            return new MoradorViewModel
            {
                Id = 1,
                Nome = "Maria Silva",
                Cpf = "12345678901",
                Rg = "123456789",
                Telefone = "11987654321",
                Email = "maria@example.com",
                Rua = "Rua A",
                Bairro = "Centro",
                Numero = "123",
                Complemento = "Apto 101",
                Cep = "12345678",
                Cidade = "São Paulo",
                Uf = "SP",
                CondominioId = 1
            };
        }

        private static MoradorViewModel GetNewMoradorModel()
        {
            return new MoradorViewModel
            {
                Nome = "João Santos",
                Cpf = "98765432101",
                Rg = "987654321",
                Telefone = "11912345678",
                Email = "joao@example.com",
                Rua = "Rua B",
                Bairro = "Vila",
                Numero = "456",
                Complemento = "Apto 202",
                Cep = "87654321",
                Cidade = "Rio de Janeiro",
                Uf = "RJ",
                CondominioId = 2
            };
        }

        private static List<MoradorViewModel> GetTestMoradores()
        {
            return new List<MoradorViewModel>
            {
                new MoradorViewModel { Id = 1, Nome = "Maria Silva", Cpf = "12345678901" },
                new MoradorViewModel { Id = 2, Nome = "João Santos", Cpf = "98765432101" },
                new MoradorViewModel { Id = 3, Nome = "Ana Costa",  Cpf = "11122233344" }
            };
        }
    }
}
