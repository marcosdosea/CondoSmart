using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace CondosmartWeb.Tests.Controllers
{
    [TestClass]
    public class PagarCondominioControllerTests
    {
        private Mock<IPagamentoService> _mockService = null!;
        private Mock<IMapper> _mockMapper = null!;
        private PagarCondominioController _controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Inicializa os mocks simulando as dependências da controller
            _mockService = new Mock<IPagamentoService>();
            _mockMapper = new Mock<IMapper>();
            
            // Injeta os mocks na Controller
            _controller = new PagarCondominioController(_mockService.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void Index_RetornaViewResult_ComListaDePagamentos()
        {
            // Arrange
            var listaPagamentos = new List<Pagamento> { new Pagamento { Id = 1, Valor = 500 } };
            var listaVm = new List<PagamentoViewModel> { new PagamentoViewModel { Id = 1, Valor = 500 } };

            _mockService.Setup(s => s.GetAll()).Returns(listaPagamentos);
            _mockMapper.Setup(m => m.Map<List<PagamentoViewModel>>(listaPagamentos)).Returns(listaVm);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual(listaVm, viewResult.Model);
        }

        [TestMethod]
        public void Details_ComIdValido_RetornaViewResultComModelo()
        {
            // Arrange
            int idBusca = 1;
            var pagamento = new Pagamento { Id = idBusca, Status = "Pago" };
            var pagamentoVm = new PagamentoViewModel { Id = idBusca, Status = "Pago" };

            _mockService.Setup(s => s.GetById(idBusca)).Returns(pagamento);
            _mockMapper.Setup(m => m.Map<PagamentoViewModel>(pagamento)).Returns(pagamentoVm);

            // Act
            var result = _controller.Details(idBusca);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual(pagamentoVm, viewResult.Model);
        }

        [TestMethod]
        public void Create_Post_ComDadosValidos_SalvaERedirecionaParaIndex()
        {
            // Arrange
            var pagamentoVm = new PagamentoViewModel { Valor = 300, FormaPagamento = "PIX" };
            var pagamento = new Pagamento { Valor = 300, FormaPagamento = "PIX" };

            _mockMapper.Setup(m => m.Map<Pagamento>(pagamentoVm)).Returns(pagamento);
            _mockService.Setup(s => s.Create(pagamento)).Verifiable();

            // Act
            var result = _controller.Create(pagamentoVm);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            _mockService.Verify(); // Garante que o método Create do Service foi chamado
        }
    }
}