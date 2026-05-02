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
    public class SindicoControllerTests
    {
        private SindicoController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<ISindicoService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestSindicos());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetSindico());
            mockService.Setup(s => s.Create(It.IsAny<Sindico>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<Sindico>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<SindicoViewModel>>(It.IsAny<List<Sindico>>())).Returns((List<Sindico> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<SindicoViewModel>(It.IsAny<Sindico>())).Returns((Sindico src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Sindico>(It.IsAny<SindicoViewModel>())).Returns((SindicoViewModel src) => ToModel(src));
            
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
            var result = controller.Create(GetNewSindicoModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        private static SindicoViewModel ToViewModel(Sindico src) => new() { Id = src.Id, Nome = src.Nome, Cpf = src.Cpf, Rua = src.Rua, Numero = src.Numero, Bairro = src.Bairro, Cidade = src.Cidade, Uf = src.Uf, Cep = src.Cep };
        private static Sindico ToModel(SindicoViewModel src) => new() { Id = src.Id, Nome = src.Nome, Cpf = src.Cpf, Rua = src.Rua, Numero = src.Numero, Bairro = src.Bairro, Cidade = src.Cidade, Uf = src.Uf, Cep = src.Cep };
        private static Sindico GetTargetSindico() => new() { Id = 1, Nome = "Joao Silva", Cpf = "12345678901", Rua = "Rua A", Numero = "10", Bairro = "Centro", Cidade = "Sao Paulo", Uf = "SP", Cep = "12345678" };
        private static SindicoViewModel GetNewSindicoModel() => new() { Id = 99, Nome = "Maria Santos", Cpf = "98765432101", Rua = "Rua Nova", Numero = "100", Bairro = "Bairro Novo", Cidade = "Rio de Janeiro", Uf = "RJ", Cep = "87654321" };
        private static List<Sindico> GetTestSindicos() => new() { GetTargetSindico() };
    }
}

