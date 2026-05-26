using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class ChamadoControllerTests
    {
        private ChamadoController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IChamadosService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockMoradorService = new Mock<IMoradorService>();
            var mockSindicoService = new Mock<ISindicoService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();

            mockService.Setup(s => s.GetAll()).Returns(GetTestChamados());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetChamado());

            mapper.Setup(m => m.Map<List<ChamadoViewModel>>(It.IsAny<List<Chamado>>())).Returns((List<Chamado> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<ChamadoViewModel>(It.IsAny<Chamado>())).Returns((Chamado src) => ToViewModel(src));

            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio>());
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);

            controller = new ChamadoController(
                mockService.Object,
                mockCondominioService.Object,
                mockMoradorService.Object,
                mockSindicoService.Object,
                mockContextService.Object,
                mockNotificacaoService.Object,
                mapper.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "teste@condo.com") }, "TestAuth"));
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var url = new Mock<IUrlHelper>();
            url.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns("/teste");
            controller.Url = url.Object;
        }

        [TestMethod]
        public void Index_Returns_View_With_List()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var model = (List<ChamadoViewModel>)((ViewResult)result).ViewData.Model!;
            Assert.AreEqual(2, model.Count);
        }

        [TestMethod]
        public void Details_InvalidId_Returns_NotFound()
        {
            var result = controller.Details(999);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_Post_InvalidModel_Returns_View()
        {
            var vm = new ChamadoViewModel { Id = 0, Descricao = "", Status = "aberto", CondominioId = 1 };
            controller.ModelState.AddModelError("Descricao", "Obrigatório");

            var result = controller.Create(vm);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ChamadoViewModel));
        }

        private static ChamadoViewModel ToViewModel(Chamado src) => new() { Id = src.Id, Descricao = src.Descricao, Status = src.Status, CondominioId = src.CondominioId };
        private static Chamado GetTargetChamado() => new() { Id = 1, Descricao = "Chamado 1", Status = "aberto", CondominioId = 1, DataChamado = DateTime.Now };
        private static List<Chamado> GetTestChamados() => new() { GetTargetChamado(), new Chamado { Id = 2, Descricao = "Chamado 2", Status = "aberto", CondominioId = 1, DataChamado = DateTime.Now } };
    }
}
