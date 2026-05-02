using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.DTO;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Http;`r`nusing Microsoft.AspNetCore.Mvc;`r`nusing Microsoft.AspNetCore.Mvc.Routing;`r`nusing Microsoft.AspNetCore.Mvc.ViewFeatures;`r`nusing System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class MoradorControllerTests
    {
        private MoradorController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IMoradorService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockUnidadesService = new Mock<IUnidadesResidenciaisService>();
            var mockProvisionamentoService = new Mock<IMoradorProvisionamentoService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestMoradores());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetMorador());
            mockService.Setup(s => s.Edit(It.IsAny<Morador>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCondominioService.Setup(s => s.GetAll()).Returns(new List<Condominio>());
            mockUnidadesService.Setup(s => s.GetAll()).Returns(new List<UnidadesResidenciais> { new() { Id = 7, Identificador = "A-101", CondominioId = 1, MoradorId = 1, Condominio = new Condominio { Id = 1, Nome = "Condominio Alfa" } }, new() { Id = 8, Identificador = "B-202", CondominioId = 1, MoradorId = null, Condominio = new Condominio { Id = 1, Nome = "Condominio Alfa" } } });
            mockUnidadesService.Setup(s => s.GetByMoradorId(1)).Returns(new UnidadesResidenciais { Id = 7, Identificador = "A-101", CondominioId = 1, MoradorId = 1, Condominio = new Condominio { Id = 1, Nome = "Condominio Alfa" } });
            mockProvisionamentoService.Setup(s => s.CadastrarComAcessoAsync(It.IsAny<Morador>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ProvisionamentoMoradorResultDTO { MoradorId = 10, NomeMorador = "Joao Santos", Email = "joao@example.com", SenhaTemporaria = "Condo@123A", Condominio = "Condominio Alfa", Unidade = "B-202", UrlAcesso = "https://localhost/login", EmailEnviado = true });
            mockProvisionamentoService.Setup(s => s.AtualizarVinculoUnidadeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            mockProvisionamentoService.Setup(s => s.AtualizarContaMoradorAsync(It.IsAny<string?>(), It.IsAny<Morador>())).Returns(Task.CompletedTask);
            mockProvisionamentoService.Setup(s => s.RemoverAcessoAsync(It.IsAny<int>(), It.IsAny<string?>())).Returns(Task.CompletedTask);
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<MoradorViewModel>>(It.IsAny<List<Morador>>())).Returns((List<Morador> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<MoradorViewModel>(It.IsAny<Morador>())).Returns((Morador src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Morador>(It.IsAny<MoradorViewModel>())).Returns((MoradorViewModel src) => ToModel(src));
            
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
            var urlMock = new Mock<IUrlHelper>();
            urlMock.Setup(u => u.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("https://localhost/login");`r`n            urlMock.Setup(u => u.Page(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("https://localhost/login");
            controller.Url = urlMock.Object;
            var result = await controller.Create(GetNewMoradorModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        private static MoradorViewModel ToViewModel(Morador src) => new() { Id = src.Id, Nome = src.Nome, Cpf = src.Cpf, Rg = src.Rg, Telefone = src.Telefone, Email = src.Email, Rua = src.Rua, Bairro = src.Bairro, Numero = src.Numero, Complemento = src.Complemento, Cep = src.Cep, Cidade = src.Cidade, Uf = src.Uf, CondominioId = src.CondominioId };
        private static Morador ToModel(MoradorViewModel src) => new() { Id = src.Id, Nome = src.Nome, Cpf = src.Cpf, Rg = src.Rg, Telefone = src.Telefone, Email = src.Email, Rua = src.Rua, Bairro = src.Bairro, Numero = src.Numero, Complemento = src.Complemento, Cep = src.Cep, Cidade = src.Cidade, Uf = src.Uf, CondominioId = src.CondominioId };
        private static Morador GetTargetMorador() => new() { Id = 1, Nome = "Maria Silva", Cpf = "12345678901", Rg = "123456789", Telefone = "11987654321", Email = "maria@example.com", Rua = "Rua A", Bairro = "Centro", Numero = "123", Complemento = "Apto 101", Cep = "12345678", Cidade = "Sao Paulo", Uf = "SP", CondominioId = 1 };
        private static MoradorViewModel GetNewMoradorModel() => new() { Nome = "Joao Santos", Cpf = "98765432101", Rg = "987654321", Telefone = "11912345678", Email = "joao@example.com", Rua = "Rua B", Bairro = "Vila", Numero = "456", Complemento = "Apto 202", Cep = "87654321", Cidade = "Rio de Janeiro", Uf = "RJ", CondominioId = 1, UnidadeId = 8 };
        private static List<Morador> GetTestMoradores() => new() { GetTargetMorador() };
    }
}


