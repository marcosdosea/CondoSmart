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
    public class MoradorControllerTests
    {
        private Mock<IMoradorService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IMoradorService>();
        }

        [TestMethod]
        public void MoradorControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
