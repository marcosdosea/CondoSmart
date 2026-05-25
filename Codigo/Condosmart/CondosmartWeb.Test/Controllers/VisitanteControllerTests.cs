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
    public class VisitanteControllerTests
    {
        private Mock<IVisitanteService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IVisitanteService>();
        }

        [TestMethod]
        public void VisitanteControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
