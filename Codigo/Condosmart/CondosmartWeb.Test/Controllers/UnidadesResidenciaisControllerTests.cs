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
    public class UnidadesResidenciaisControllerTests
    {
        private UnidadesResidenciaisController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IUnidadesResidenciaisService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockCepService = new Mock<ICepService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestUnidades());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetUnidade());
            mockService.Setup(s => s.Create(It.IsAny<UnidadesResidenciais>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<UnidadesResidenciais>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCondominioService.Setup(s => s.GetAll()).Returns(GetTestCondominios());
            mockCepService.Setup(s => s.IsValidAsync(It.IsAny<string?>())).ReturnsAsync(true);
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<UnidadeResidencialViewModel>>(It.IsAny<List<UnidadesResidenciais>>())).Returns((List<UnidadesResidenciais> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<UnidadeResidencialViewModel>(It.IsAny<UnidadesResidenciais>())).Returns((UnidadesResidenciais src) => ToViewModel(src));
            mapper.Setup(m => m.Map<UnidadesResidenciais>(It.IsAny<UnidadeResidencialViewModel>())).Returns((UnidadeResidencialViewModel src) => ToModel(src));
            
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, 'teste@condo.com') }, 'TestAuth'));
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var url = new Mock<IUrlHelper>();
            url.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns('/teste');
            controller.Url = url.Object;
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            var result = await controller.Create(GetNewUnidadeModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        private static UnidadeResidencialViewModel ToViewModel(UnidadesResidenciais src) => new() { Id = src.Id, Identificador = src.Identificador, Rua = src.Rua, Numero = src.Numero, Bairro = src.Bairro, Complemento = src.Complemento, Cep = src.Cep, Cidade = src.Cidade, Uf = src.Uf, TelefoneResidencial = src.TelefoneResidencial, TelefoneCelular = src.TelefoneCelular, MoradorId = src.MoradorId, CondominioId = src.CondominioId, SindicoId = src.SindicoId };
        private static UnidadesResidenciais ToModel(UnidadeResidencialViewModel src) => new() { Id = src.Id, Identificador = src.Identificador, Rua = src.Rua, Numero = src.Numero, Bairro = src.Bairro, Complemento = src.Complemento, Cep = src.Cep, Cidade = src.Cidade, Uf = src.Uf, TelefoneResidencial = src.TelefoneResidencial, TelefoneCelular = src.TelefoneCelular, MoradorId = src.MoradorId, CondominioId = src.CondominioId, SindicoId = src.SindicoId };
        private static UnidadesResidenciais GetTargetUnidade() => new() { Id = 1, Identificador = "Bloco A - Apto 101", Rua = "Rua das Flores", Numero = "100", Bairro = "Centro", Cep = "12345678", Cidade = "Sao Paulo", Uf = "SP", TelefoneResidencial = "11933334444", TelefoneCelular = "11987654321", CondominioId = 1, Condominio = new Condominio { Id = 1, Nome = "Condominio Alfa" } };
        private static UnidadeResidencialViewModel GetNewUnidadeModel() => new() { Id = 99, Identificador = "Bloco B - Apto 202", Rua = "Rua das Acacias", Numero = "200", Bairro = "Jardim Paulista", Complemento = "Proximo ao shopping", Cep = "87654321", Cidade = "Sao Paulo", Uf = "SP", TelefoneResidencial = "11944445555", TelefoneCelular = "11912345678", CondominioId = 1, SindicoId = 1 };
        private static List<UnidadesResidenciais> GetTestUnidades() => new() { GetTargetUnidade() };
        private static List<Condominio> GetTestCondominios() => new() { new() { Id = 1, Nome = "Condominio Alfa", Cnpj = "12345678901234", Rua = "Rua A", Numero = "10", Bairro = "Centro", Cidade = "Cidade", Uf = "SP", Cep = "12345678" } };
    }
}

