using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Mappers;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class AtaControllerTests
    {
        private static AtaController controller = null!;
        private static Mock<ICondominioService> mockCondominioService = null!;
        private static Mock<ISindicoService> mockSindicoService = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IAtaService>();
            mockCondominioService = new Mock<ICondominioService>();
            mockSindicoService = new Mock<ISindicoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AtaProfile())
            ).CreateMapper();

            mockService.Setup(s => s.GetAll())
                .Returns(GetTestAtas());

            mockService.Setup(s => s.GetById(1))
                .Returns(GetTargetAta());

            mockService.Setup(s => s.Edit(It.IsAny<Ata>()))
                .Verifiable();

            mockService.Setup(s => s.Create(It.IsAny<Ata>()))
                .Returns(10);

            mockService.Setup(s => s.Delete(It.IsAny<int>()))
                .Verifiable();

            mockCondominioService.Setup(s => s.GetAll())
                .Returns(GetTestCondominios());

            mockSindicoService.Setup(s => s.GetAll())
                .Returns(GetTestSindicos());

            controller = new AtaController(mockService.Object, mockCondominioService.Object, mockSindicoService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<AtaViewModel>));
            var lista = (List<AtaViewModel>)viewResult.ViewData.Model;

            Assert.HasCount(3, lista);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AtaViewModel));
            var model = (AtaViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Ata de Reunião Ordinária", model.Titulo);
            Assert.AreEqual("Manutenção, Obras", model.Temas);
        }

        [TestMethod]
        public void CreateTest_Get_Valido()
        {
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CreateTest_Post_Valid()
        {
            // Act
            var result = controller.Create(GetNewAtaModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CreateTest_Post_Invalid()
        {
            // Arrange
            controller.ModelState.AddModelError("Titulo", "Campo requerido");

            // Act
            var result = controller.Create(GetNewAtaModel());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditTest_Get_Valid()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AtaViewModel));
            var model = (AtaViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Ata de Reunião Ordinária", model.Titulo);
            Assert.AreEqual("Manutenção, Obras", model.Temas);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(GetTargetAtaModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Get_Valid()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AtaViewModel));
            var model = (AtaViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("Ata de Reunião Ordinária", model.Titulo);
        }

        [TestMethod]
        public void DeleteTest_Post_Valid()
        {
            // Act
            var result = controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;

            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        // --------- Dados de Teste ---------

        private static Ata GetTargetAta()
        {
            return new Ata
            {
                Id = 1,
                Titulo = "Ata de Reunião Ordinária",
                Temas = "Manutenção, Obras",
                Conteudo = "Conteúdo da ata de reunião ordinária...",
                DataReuniao = new DateOnly(2024, 1, 15),
                CondominioId = 1,
                SindicoId = 1
            };
        }

        private AtaViewModel GetTargetAtaModel()
        {
            return new AtaViewModel
            {
                Id = 1,
                Titulo = "Ata de Reunião Ordinária",
                Temas = "Manutenção, Obras",
                Conteudo = "Conteúdo da ata de reunião ordinária...",
                DataReuniao = new DateTime(2024, 1, 15),
                CondominioId = 1,
                SindicoId = 1
            };
        }

        private AtaViewModel GetNewAtaModel()
        {
            return new AtaViewModel
            {
                Id = 99,
                Titulo = "Ata de Reunião Extraordinária",
                Temas = "Eleição de Síndico",
                Conteudo = "Conteúdo da ata de reunião extraordinária...",
                DataReuniao = new DateTime(2024, 2, 20),
                CondominioId = 1,
                SindicoId = 1
            };
        }

        private List<Ata> GetTestAtas()
        {
            return new List<Ata>
            {
                new Ata
                {
                    Id = 1,
                    Titulo = "Ata de Reunião Ordinária",
                    Temas = "Manutenção, Obras",
                    Conteudo = "Conteúdo da ata de reunião ordinária...",
                    DataReuniao = new DateOnly(2024, 1, 15),
                    CondominioId = 1,
                    SindicoId = 1
                },
                new Ata
                {
                    Id = 2,
                    Titulo = "Ata de Reunião Extraordinária",
                    Temas = "Eleição de Síndico",
                    Conteudo = "Conteúdo da ata de reunião extraordinária...",
                    DataReuniao = new DateOnly(2024, 2, 20),
                    CondominioId = 1,
                    SindicoId = 1
                },
                new Ata
                {
                    Id = 3,
                    Titulo = "Ata de Assembleia Geral",
                    Temas = "Aprovação de Contas",
                    Conteudo = "Conteúdo da ata de assembleia geral...",
                    DataReuniao = new DateOnly(2024, 3, 10),
                    CondominioId = 2,
                    SindicoId = 2
                }
            };
        }

        private List<Condominio> GetTestCondominios()
        {
            return new List<Condominio>
            {
                new Condominio
                {
                    Id = 1,
                    Nome = "Condomínio Alfa",
                    Cnpj = "12345678901234",
                    Rua = "Rua A",
                    Numero = "10",
                    Bairro = "Centro",
                    Cidade = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Unidades = 10
                },
                new Condominio
                {
                    Id = 2,
                    Nome = "Condomínio Beta",
                    Cnpj = "22345678901234",
                    Rua = "Rua B",
                    Numero = "20",
                    Bairro = "Bairro B",
                    Cidade = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Unidades = 15
                }
            };
        }

        private List<Sindico> GetTestSindicos()
        {
            return new List<Sindico>
            {
                new Sindico
                {
                    Id = 1,
                    Nome = "João Silva",
                    Cpf = "12345678901",
                    Rua = "Rua X",
                    Numero = "100",
                    Bairro = "Centro",
                    Cidade = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Email = "joao@email.com",
                    Telefone = "11987654321"
                },
                new Sindico
                {
                    Id = 2,
                    Nome = "Maria Santos",
                    Cpf = "98765432109",
                    Rua = "Rua Y",
                    Numero = "200",
                    Bairro = "Bairro B",
                    Cidade = "Cidade",
                    Uf = "SP",
                    Cep = "12345678",
                    Email = "maria@email.com",
                    Telefone = "11912345678"
                }
            };
        }
    }
}
