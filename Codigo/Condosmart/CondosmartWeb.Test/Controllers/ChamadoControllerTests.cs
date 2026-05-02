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
            mockService.Setup(s => s.Create(It.IsAny<Chamado>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<Chamado>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCondominioService.Setup(s => s.GetById(It.IsAny<int>())).Returns((int id) => id == 1 ? new Condominio { Id = 1, Nome = "Condominio Teste" } : null);
            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio> { new() { Id = 1, Nome = "Condominio Teste" } });
            mockMoradorService.Setup(s => s.GetAll()).Returns(new List<Morador>());
            mockSindicoService.Setup(s => s.GetAll()).Returns(new List<Sindico>());
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<ChamadoViewModel>>(It.IsAny<List<Chamado>>())).Returns((List<Chamado> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<ChamadoViewModel>(It.IsAny<Chamado>())).Returns((Chamado src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Chamado>(It.IsAny<ChamadoViewModel>())).Returns((ChamadoViewModel src) => ToModel(src));
            
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, 'teste@condo.com') }, 'TestAuth'));
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var url = new Mock<IUrlHelper>();
            url.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns('/teste');
            controller.Url = url.Object;
        }

        [TestMethod]
        public void Index_ReturnsView_WithList()
        {
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var model = (List<ChamadoViewModel>)((ViewResult)result).ViewData.Model!;
            Assert.HasCount(3, model);
        }

        [TestMethod]
        public void Create_Post_Valid_RedirectsToIndex()
        {
            var result = controller.Create(GetNewChamadoModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        private static ChamadoViewModel ToViewModel(Chamado src) => new() { Id = src.Id, Descricao = src.Descricao, Status = src.Status, MoradorId = src.MoradorId, SindicoId = src.SindicoId, CondominioId = src.CondominioId, DataChamado = src.DataChamado ?? DateTime.Now };
        private static Chamado ToModel(ChamadoViewModel src) => new() { Id = src.Id, Descricao = src.Descricao, Status = src.Status, MoradorId = src.MoradorId, SindicoId = src.SindicoId, CondominioId = src.CondominioId, DataChamado = src.DataChamado };
        private static Chamado GetTargetChamado() => new() { Id = 1, Descricao = "Vazamento na cozinha", Status = "aberto", MoradorId = 5, CondominioId = 1, DataChamado = DateTime.Now };
        private static ChamadoViewModel GetNewChamadoModel() => new() { Id = 99, Descricao = "Luz queimada corredor", Status = "aberto", MoradorId = 2, CondominioId = 1, DataChamado = DateTime.Now };
        private static List<Chamado> GetTestChamados() => new() { GetTargetChamado(), new Chamado { Id = 2, Descricao = "Porta emperrada", Status = "em_andamento", CondominioId = 1, DataChamado = DateTime.Now }, new Chamado { Id = 3, Descricao = "Luz corredor", Status = "resolvido", CondominioId = 1, DataChamado = DateTime.Now } };
    }
}

