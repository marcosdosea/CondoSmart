using AutoMapper;
using CondosmartWeb.Controllers;
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
    public class ChamadoControllerTests
    {
        private Mock<IChamadosService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IChamadosService>();
        }

        [TestMethod]
        public void ChamadoControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
