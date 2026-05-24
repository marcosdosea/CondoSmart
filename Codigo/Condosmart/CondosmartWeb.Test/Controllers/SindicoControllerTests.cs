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
    public class SindicoControllerTests
    {
        private Mock<ISindicoService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ISindicoService>();
        }

        [TestMethod]
        public void SindicoControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
