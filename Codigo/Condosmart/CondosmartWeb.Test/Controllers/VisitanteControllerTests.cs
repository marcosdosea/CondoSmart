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
    public class VisitanteControllerTests
    {
        private VisitanteController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IVisitanteService>();
            var mockMoradorService = new Mock<IMoradorService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestVisitantes());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetVisitante());
            mockService.Setup(s => s.Create(It.IsAny<Visitantes>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<Visitantes>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockMoradorService.Setup(s => s.GetAll()).Returns(GetTestMoradores());
            mockMoradorService.Setup(s => s.GetById(It.IsAny<int>())).Returns((int id) => GetTestMoradores().FirstOrDefault(m => m.Id == id));
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<VisitanteViewModel>>(It.IsAny<List<Visitantes>>())).Returns((List<Visitantes> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<VisitanteViewModel>(It.IsAny<Visitantes>())).Returns((Visitantes src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Visitantes>(It.IsAny<VisitanteViewModel>())).Returns((VisitanteViewModel src) => ToModel(src));
            
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
            var result = controller.Create(GetNewVisitanteModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        private static VisitanteViewModel ToViewModel(Visitantes src) => new() { Id = src.Id, Nome = src.Nome, Cpf = src.Cpf, Telefone = src.Telefone, MoradorId = src.MoradorId, Observacao = src.Observacao, DataHoraEntrada = src.DataHoraEntrada, DataHoraSaida = src.DataHoraSaida };
        private static Visitantes ToModel(VisitanteViewModel src) => new() { Id = src.Id, Nome = src.Nome, Cpf = src.Cpf, Telefone = src.Telefone, MoradorId = src.MoradorId, Observacao = src.Observacao, DataHoraEntrada = src.DataHoraEntrada, DataHoraSaida = src.DataHoraSaida };
        private static Visitantes GetTargetVisitante() => new() { Id = 1, Nome = "Joao Silva", Cpf = "12345678901", Telefone = "11987654321", MoradorId = 1 };
        private static VisitanteViewModel GetNewVisitanteModel() => new() { Id = 99, Nome = "Maria Santos", Cpf = "98765432109", Telefone = "11912345678", MoradorId = 1 };
        private static List<Visitantes> GetTestVisitantes() => new() { GetTargetVisitante() };
        private static List<Morador> GetTestMoradores() => new() { new Morador { Id = 1, Nome = "Ana Silva", Cpf = "11122233344", CondominioId = 1 } };
    }
}

