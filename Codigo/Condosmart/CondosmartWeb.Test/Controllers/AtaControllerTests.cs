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
    public class AtaControllerTests
    {
        private AtaController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IAtaService>();
            var mockCondominioService = new Mock<ICondominioService>();
            var mockSindicoService = new Mock<ISindicoService>();
            var mockContextService = new Mock<ICondominioContextService>();
            var mockUploadService = new Mock<IArquivoUploadService>();
            var mockNotificacaoService = new Mock<INotificacaoService>();
            var mapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetAll()).Returns(GetTestAtas());
            mockService.Setup(s => s.GetById(1)).Returns(GetTargetAta());
            mockService.Setup(s => s.Create(It.IsAny<Ata>())).Returns(10);
            mockService.Setup(s => s.Edit(It.IsAny<Ata>()));
            mockService.Setup(s => s.Delete(It.IsAny<int>()));
            mockCondominioService.Setup(s => s.GetAll()).Returns(GetTestCondominios());
            mockSindicoService.Setup(s => s.GetAll()).Returns(GetTestSindicos());
            mockContextService.Setup(s => s.GetCondominioAtualId()).Returns(1);
            mapper.Setup(m => m.Map<List<AtaViewModel>>(It.IsAny<List<Ata>>())).Returns((List<Ata> src) => src.Select(ToViewModel).ToList());
            mapper.Setup(m => m.Map<AtaViewModel>(It.IsAny<Ata>())).Returns((Ata src) => ToViewModel(src));
            mapper.Setup(m => m.Map<Ata>(It.IsAny<AtaViewModel>())).Returns((AtaViewModel src) => ToModel(src));
            
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
            var model = (List<AtaViewModel>)((ViewResult)result).ViewData.Model!;
            Assert.HasCount(2, model);
        }

        [TestMethod]
        public async Task CreateTest_Post_Valid()
        {
            var result = await controller.Create(GetNewAtaModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [TestMethod]
        public async Task EditTest_Post_Valid()
        {
            var result = await controller.Edit(GetTargetAtaModel());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        private static AtaViewModel ToViewModel(Ata src) => new() { Id = src.Id, Titulo = src.Titulo ?? string.Empty, Temas = src.Temas ?? string.Empty, Conteudo = src.Conteudo ?? string.Empty, DataReuniao = src.DataReuniao?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today, CondominioId = src.CondominioId, SindicoId = src.SindicoId };
        private static Ata ToModel(AtaViewModel src) => new() { Id = src.Id, Titulo = src.Titulo, Temas = src.Temas, Conteudo = src.Conteudo, DataReuniao = DateOnly.FromDateTime(src.DataReuniao), CondominioId = src.CondominioId, SindicoId = src.SindicoId };
        private static Ata GetTargetAta() => new() { Id = 1, Titulo = "Ata Ordinaria", Temas = "Manutencao", Conteudo = "Conteudo de teste suficiente.", DataReuniao = new DateOnly(2024, 1, 15), CondominioId = 1, SindicoId = 1 };
        private static AtaViewModel GetTargetAtaModel() => new() { Id = 1, Titulo = "Ata Ordinaria", Temas = "Manutencao", Conteudo = "Conteudo de teste suficiente.", DataReuniao = new DateTime(2024, 1, 15), CondominioId = 1, SindicoId = 1 };
        private static AtaViewModel GetNewAtaModel() => new() { Id = 2, Titulo = "Ata Extraordinaria", Temas = "Eleicao", Conteudo = "Conteudo de teste suficiente para criacao.", DataReuniao = new DateTime(2024, 2, 20), CondominioId = 1, SindicoId = 1 };
        private static List<Ata> GetTestAtas() => new() { GetTargetAta(), new Ata { Id = 2, Titulo = "Ata Assembleia", Temas = "Contas", Conteudo = "Conteudo de teste suficiente.", DataReuniao = new DateOnly(2024, 3, 10), CondominioId = 1, SindicoId = 1 } };
        private static List<Condominio> GetTestCondominios() => new() { new() { Id = 1, Nome = "Condominio Alfa", Cnpj = "12345678901234", Rua = "Rua A", Numero = "10", Bairro = "Centro", Cidade = "Cidade", Uf = "SP", Cep = "12345678" } };
        private static List<Sindico> GetTestSindicos() => new() { new() { Id = 1, Nome = "Joao Silva", Cpf = "12345678901", Rua = "Rua X", Numero = "100", Bairro = "Centro", Cidade = "Cidade", Uf = "SP", Cep = "12345678", Email = "joao@email.com", Telefone = "11987654321" } };
    }
}

