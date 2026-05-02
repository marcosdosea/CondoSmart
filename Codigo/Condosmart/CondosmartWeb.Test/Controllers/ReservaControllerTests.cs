using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Http;`r`nusing Microsoft.AspNetCore.Mvc;`r`nusing Microsoft.AspNetCore.Mvc.Routing;`r`nusing Microsoft.AspNetCore.Mvc.ViewFeatures;`r`nusing System.Security.Claims;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class ReservaControllerTests
    {
        private ReservaController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IReservaService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockAreaService = new Mock<IAreaDeLazerService>();
            var mockMoradorService = new Mock<IMoradorService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestReservas());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetReserva());
            mockService.Setup(s => s.Create(It.IsAny<Reserva>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<Reserva>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio>());
            mockAreaService.Setup(s => s.GetAll()).Returns(new List<AreaDeLazer>());
            mockMoradorService.Setup(s => s.GetAll()).Returns(new List<Morador>());
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<ReservaViewModel>>(It.IsAny<List<Reserva>>())).Returns((List<Reserva> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<ReservaViewModel>(It.IsAny<Reserva>())).Returns((Reserva src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Reserva>(It.IsAny<ReservaViewModel>())).Returns((ReservaViewModel src) => ToModel(src));
            
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, 'teste@condo.com') }, 'TestAuth'));
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var url = new Mock<IUrlHelper>();
            url.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns('/teste');
            controller.Url = url.Object;
        }

        [TestMethod]
        public void CreateTest_Post_Valid()
        {
            var result = controller.Create(GetNewReservaModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        private static ReservaViewModel ToViewModel(Reserva src) => new() { Id = src.Id, AreaId = src.AreaId, CondominioId = src.CondominioId, DataInicio = src.DataInicio, DataFim = src.DataFim, MoradorId = src.MoradorId, Status = src.Status };
        private static Reserva ToModel(ReservaViewModel src) => new() { Id = src.Id, AreaId = src.AreaId, CondominioId = src.CondominioId, DataInicio = src.DataInicio, DataFim = src.DataFim, MoradorId = src.MoradorId, Status = src.Status };
        private static Reserva GetTargetReserva() => new() { Id = 1, AreaId = 2, CondominioId = 1, DataInicio = new DateTime(2026, 1, 1, 10, 0, 0), DataFim = new DateTime(2026, 1, 1, 12, 0, 0), MoradorId = 5, Status = "confirmado" };
        private static ReservaViewModel GetNewReservaModel() => new() { Id = 99, AreaId = 3, CondominioId = 1, DataInicio = DateTime.UtcNow.AddDays(1), DataFim = DateTime.UtcNow.AddDays(1).AddHours(2), MoradorId = 6, Status = "pendente" };
        private static List<Reserva> GetTestReservas() => new() { GetTargetReserva(), new Reserva { Id = 2, AreaId = 3, CondominioId = 1, DataInicio = DateTime.UtcNow.AddDays(2), DataFim = DateTime.UtcNow.AddDays(2).AddHours(1), Status = "pendente" }, new Reserva { Id = 3, AreaId = 4, CondominioId = 1, DataInicio = DateTime.UtcNow.AddDays(3), DataFim = DateTime.UtcNow.AddDays(3).AddHours(1), Status = "cancelado" } };
    }
}

