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
    public class AreaDeLazerControllerTests
    {
        private AreaDeLazerController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IAreaDeLazerService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockSindicoService = new Mock<ISindicoService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockUploadService = new Mock<IArquivoUploadService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestAreasDeLazer());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetAreaDeLazer());
            mockService.Setup(s => s.Create(It.IsAny<AreaDeLazer>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<AreaDeLazer>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio>());
            mockSindicoService.Setup(s => s.GetAll()).Returns(new List<Sindico>());
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<AreaDeLazerViewModel>>(It.IsAny<List<AreaDeLazer>>())).Returns((List<AreaDeLazer> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<AreaDeLazerViewModel>(It.IsAny<AreaDeLazer>())).Returns((AreaDeLazer src) => ToViewModel(src));
            mapper.Setup(m => m.Map<AreaDeLazer>(It.IsAny<AreaDeLazerViewModel>())).Returns((AreaDeLazerViewModel src) => ToModel(src));
            
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, 'teste@condo.com') }, 'TestAuth'));
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var url = new Mock<IUrlHelper>();
            url.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns('/teste');
            controller.Url = url.Object;
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var model = (List<AreaDeLazerViewModel>)((ViewResult)result).ViewData.Model!;
            Assert.HasCount(2, model);
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            var result = await controller.Create(GetNewAreaDeLazerModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [TestMethod]
        public async Task EditTest_Post_Invalid_IdMismatch()
        {
            var model = GetTargetAreaDeLazerModel();
            model.Id = 2;
            var result = await controller.Edit(1, model);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        private static AreaDeLazerViewModel ToViewModel(AreaDeLazer src) => new() { Id = src.Id, Nome = src.Nome, Descricao = src.Descricao, Disponibilidade = src.Disponibilidade, CondominioId = src.CondominioId, SindicoId = src.SindicoId };
        private static AreaDeLazer ToModel(AreaDeLazerViewModel src) => new() { Id = src.Id, Nome = src.Nome, Descricao = src.Descricao, Disponibilidade = src.Disponibilidade, CondominioId = src.CondominioId, SindicoId = src.SindicoId };
        private static AreaDeLazer GetTargetAreaDeLazer() => new() { Id = 1, Nome = "Piscina", Descricao = "Piscina aquecida com raias para natacao", Disponibilidade = true, CondominioId = 1, SindicoId = 1, CreatedAt = new DateTime(2024, 1, 15) };
        private static AreaDeLazerViewModel GetTargetAreaDeLazerModel() => new() { Id = 1, Nome = "Piscina", Descricao = "Piscina aquecida com raias para natacao", Disponibilidade = true, CondominioId = 1, SindicoId = 1 };
        private static AreaDeLazerViewModel GetNewAreaDeLazerModel() => new() { Id = 2, Nome = "Quadra", Descricao = "Quadra coberta para esportes", Disponibilidade = true, CondominioId = 1, SindicoId = 1 };
        private static List<AreaDeLazer> GetTestAreasDeLazer() => new() { GetTargetAreaDeLazer(), new AreaDeLazer { Id = 2, Nome = "Sauna", Descricao = "Sauna seca", Disponibilidade = false, CondominioId = 1, SindicoId = 1 } };
    }
}

