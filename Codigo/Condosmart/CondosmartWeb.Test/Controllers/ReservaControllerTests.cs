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
    public class ReservaControllerTests
    {
        private static ReservaController CreateController(params HttpResponseMessage[] responses)
        {
            var queue = new Queue<HttpResponseMessage>(responses);
            var handler = new FakeHttpMessageHandler(queue);
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient("CondoSmartAPI")).Returns(httpClient);
            return new ReservaController(mockFactory.Object);
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
            var controller = CreateController(JsonResponse(GetTestReservasVM()));
            var result = await controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ReservaViewModel>));
            var lista = (List<ReservaViewModel>)viewResult.ViewData.Model!;
            Assert.HasCount(3, lista);
        }

        [TestMethod]
        public async Task DetailsTest_Valido()
        {
            var controller = CreateController(JsonResponse(GetTargetReservaModel()));
            var result = await controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ReservaViewModel));
            var model = (ReservaViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(2, model.AreaId);
            Assert.AreEqual(1, model.CondominioId);
        }

        [TestMethod]
        public async Task CreateTest_Get_Valido()
        {
            // CarregarListas faz 2 chamadas: GET api/areadelazer + GET api/moradores
            var controller = CreateController(
                JsonResponse(new List<AreaDeLazerViewModel>()),
                JsonResponse(new List<MoradorViewModel>()));

            var result = await controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            var controller = CreateController(JsonResponse(GetNewReservaModel(), HttpStatusCode.Created));
            var result = await controller.Create(GetNewReservaModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task CreateTest_Post_Invalid()
        {
            // ModelState inválido: pula o POST mas chama CarregarListas (2 chamadas)
            var controller = CreateController(
                JsonResponse(new List<AreaDeLazerViewModel>()),
                JsonResponse(new List<MoradorViewModel>()));
            controller.ModelState.AddModelError("AreaId", "Campo requerido");

            var result = await controller.Create(GetNewReservaModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task EditTest_Get_Valid()
        {
            // 3 chamadas: GET api/reservas/{id} + GET api/areadelazer + GET api/moradores
            var controller = CreateController(
                JsonResponse(GetTargetReservaModel()),
                JsonResponse(new List<AreaDeLazerViewModel>()),
                JsonResponse(new List<MoradorViewModel>()));

            var result = await controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ReservaViewModel));
            var model = (ReservaViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(2, model.AreaId);
        }

        [TestMethod]
        public async Task EditTest_Post_Valid()
        {
            var controller = CreateController(JsonResponse(GetTargetReservaModel()));
            var model = GetTargetReservaModel();
            var result = await controller.Edit(model.Id, model);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public async Task DeleteTest_Get_Valid()
        {
            var controller = CreateController(JsonResponse(GetTargetReservaModel()));
            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ReservaViewModel));
            var model = (ReservaViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual(1, model.Id);
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

        private static ReservaViewModel GetTargetReservaModel() => new()
        {
            Id = 1,
            AreaId = 2,
            CondominioId = 1,
            DataInicio = new DateTime(2026, 01, 01, 10, 0, 0),
            DataFim = new DateTime(2026, 01, 01, 12, 0, 0),
            MoradorId = 5,
            Status = "confirmado"
        };

        private static ReservaViewModel GetNewReservaModel() => new()
        {
            Id = 99,
            AreaId = 3,
            CondominioId = 1,
            DataInicio = DateTime.UtcNow.AddDays(1),
            DataFim = DateTime.UtcNow.AddDays(1).AddHours(2),
            MoradorId = 6,
            Status = "pendente"
        };

        private static List<ReservaViewModel> GetTestReservasVM() => new()
        {
            GetTargetReservaModel(),
            new ReservaViewModel { Id = 2, AreaId = 3, CondominioId = 1, DataInicio = DateTime.UtcNow.AddDays(2), DataFim = DateTime.UtcNow.AddDays(2).AddHours(3), MoradorId = 4, Status = "pendente" },
            new ReservaViewModel { Id = 3, AreaId = 4, CondominioId = 1, DataInicio = DateTime.UtcNow.AddDays(3), DataFim = DateTime.UtcNow.AddDays(3).AddHours(1), MoradorId = 2, Status = "cancelado" }
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

