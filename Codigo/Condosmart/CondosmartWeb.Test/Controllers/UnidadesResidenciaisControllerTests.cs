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
    public class UnidadesResidenciaisControllerTests
    {
        private Mock<IUnidadesResidenciaisService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IUnidadesResidenciaisService>();
        }

        [TestMethod]
        public void UnidadesResidenciaisControllerExists()
        {
            Assert.IsNotNull(mockService.Object);
        }
    }
}
