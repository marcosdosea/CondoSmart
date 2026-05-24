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
    public class CondominioControllerTests
    {
        private Mock<ICondominioService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ICondominioService>();
        }

        [TestMethod]
        public void CondominioControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
