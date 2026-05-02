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
    public class CondominioControllerTests
    {
        private CondominioController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<ICondominioService>();
            var mockCnpjService = new Mock<ICnpjService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestCondominios());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetCondominio());
            mockService.Setup(s => s.Create(It.IsAny<Condominio>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<Condominio>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCnpjService.Setup(s => s.IsValid(It.IsAny<string?>())).Returns(true);
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<CondominioViewModel>>(It.IsAny<List<Condominio>>())).Returns((List<Condominio> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<CondominioViewModel>(It.IsAny<Condominio>())).Returns((Condominio src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Condominio>(It.IsAny<CondominioViewModel>())).Returns((CondominioViewModel src) => ToModel(src));
            
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
            var model = (PagedListViewModel<CondominioViewModel>)((ViewResult)result).ViewData.Model!;
            Assert.AreEqual(3, model.TotalItems);
        }

        [TestMethod]
        public void CreateTest_Post_Valid()
        {
            var result = controller.Create(GetNewCondominioModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        private static CondominioViewModel ToViewModel(Condominio src) => new() { Id = src.Id, Nome = src.Nome, Cnpj = src.Cnpj ?? string.Empty, Rua = src.Rua ?? string.Empty, Numero = src.Numero ?? string.Empty, Bairro = src.Bairro ?? string.Empty, Cidade = src.Cidade ?? string.Empty, Uf = src.Uf ?? string.Empty, Cep = src.Cep ?? string.Empty, Unidades = src.Unidades ?? 0 };
        private static Condominio ToModel(CondominioViewModel src) => new() { Id = src.Id, Nome = src.Nome, Cnpj = src.Cnpj, Rua = src.Rua, Numero = src.Numero, Bairro = src.Bairro, Cidade = src.Cidade, Uf = src.Uf, Cep = src.Cep, Unidades = src.Unidades };
        private static Condominio GetTargetCondominio() => new() { Id = 1, Nome = "Condominio Alfa", Cnpj = "12345678901234", Rua = "Rua A", Numero = "10", Bairro = "Centro", Cidade = "Cidade", Uf = "SP", Cep = "12345678", Unidades = 10 };
        private static CondominioViewModel GetNewCondominioModel() => new() { Id = 99, Nome = "Condominio Novo", Cnpj = "99999999999999", Rua = "Rua Nova", Numero = "100", Bairro = "Bairro Novo", Cidade = "Cidade Nova", Uf = "BA", Cep = "87654321", Unidades = 20 };
        private static List<Condominio> GetTestCondominios() => new() { GetTargetCondominio(), new Condominio { Id = 2, Nome = "Condominio Beta", Cnpj = "22345678901234", Rua = "Rua B", Numero = "20", Bairro = "B", Cidade = "Cidade", Uf = "SP", Cep = "12345678", Unidades = 15 }, new Condominio { Id = 3, Nome = "Condominio Gama", Cnpj = "32345678901234", Rua = "Rua C", Numero = "30", Bairro = "C", Cidade = "Cidade", Uf = "RJ", Cep = "12345678", Unidades = 8 } };
    }
}




