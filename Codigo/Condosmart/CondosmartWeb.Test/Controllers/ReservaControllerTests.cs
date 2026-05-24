using AutoMapper;
using CondosmartWeb.Controllers;
using CondosmartWeb.Models;
using CondosmartWeb.Services;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class ReservaControllerTests
    {
        private Mock<IReservaService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IReservaService>();
            mockService.Setup(s => s.GetAll()).Returns(new List<Reserva>());
        }

        [TestMethod]
        public void GetAll_ReturnsListOfReservas()
        {
            var reservas = mockService.Object.GetAll();
            Assert.IsNotNull(reservas);
        }
    }
}
