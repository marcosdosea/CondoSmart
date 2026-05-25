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
    public class AtaControllerTests
    {
        private Mock<IAtaService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IAtaService>();
        }

        [TestMethod]
        public void AtaControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
