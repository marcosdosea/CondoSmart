using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MoradorControllerTests
    {
        private static MoradorController CreateController(params HttpResponseMessage[] responses)
        {
            var queue = new Queue<HttpResponseMessage>(responses);
            var handler = new FakeHttpMessageHandler(queue);
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient("CondoSmartAPI")).Returns(httpClient);
            return new MoradorController(mockFactory.Object);
        }

        private static HttpResponseMessage JsonResponse<T>(T data, HttpStatusCode status = HttpStatusCode.OK)
        {
            var json = JsonSerializer.Serialize(data);
            return new HttpResponseMessage(status)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        [TestMethod]
        public async Task IndexTest_Valido()
        {
            var controller = CreateController(JsonResponse(GetTestMoradoresVM()));
            var result = await controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<MoradorViewModel>));
            var lista = (List<MoradorViewModel>)viewResult.ViewData.Model!;
            Assert.HasCount(3, lista);
        }

        [TestMethod]
        public async Task DetailsTest_Valido()
        {
            var controller = CreateController(JsonResponse(GetTargetMoradorModel()));
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
            var controller = CreateController();
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            var controller = CreateController(JsonResponse(GetNewMoradorModel(), HttpStatusCode.Created));
            var result = await controller.Create(GetNewMoradorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task CreateTest_Post_Invalid()
        {
            var controller = CreateController();
            controller.ModelState.AddModelError("Nome", "Campo requerido");
            var result = await controller.Create(GetNewMoradorModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task EditTest_Get_Valid()
        {
            var controller = CreateController(JsonResponse(GetTargetMoradorModel()));
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
            var controller = CreateController(JsonResponse(GetTargetMoradorModel()));
            var result = await controller.Edit(GetTargetMoradorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task DeleteTest_Get_Valid()
        {
            var controller = CreateController(JsonResponse(GetTargetMoradorModel()));
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
            var controller = CreateController(new HttpResponseMessage(HttpStatusCode.OK));
            var result = await controller.DeleteConfirmed(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        // --------- Dados de Teste ---------

        private static MoradorViewModel GetTargetMoradorModel() => new()
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

        private static MoradorViewModel GetNewMoradorModel() => new()
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

        private static List<MoradorViewModel> GetTestMoradoresVM() => new()
        {
            GetTargetMoradorModel(),
            new MoradorViewModel { Id = 2, Nome = "João Santos", Cpf = "98765432101", CondominioId = 1 },
            new MoradorViewModel { Id = 3, Nome = "Ana Costa",   Cpf = "55555555555", CondominioId = 2 }
        };

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly Queue<HttpResponseMessage> _responses;

            public FakeHttpMessageHandler(Queue<HttpResponseMessage> responses) =>
                _responses = responses;

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken) =>
                Task.FromResult(_responses.Count > 0
                    ? _responses.Dequeue()
                    : new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
