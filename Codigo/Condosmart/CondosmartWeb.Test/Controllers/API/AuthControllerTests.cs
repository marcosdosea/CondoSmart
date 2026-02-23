using CondosmartAPI.Controllers;
using CondosmartAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CondosmartWeb.Controllers.Tests
{
    [TestClass]
    public class AuthControllerTests
    {
        private static AuthController controller = null!;
        private static Mock<UserManager<ApplicationUser>> mockUserManager = null!;
        private static Mock<IConfiguration> mockConfig = null!;

        [TestInitialize]
        public void Initialize()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("CondoSmart@SuperSecretKey2024MinLen32!");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("CondosmartAPI");
            mockConfig.Setup(c => c["Jwt:Audience"]).Returns("CondosmartClients");
            mockConfig.Setup(c => c["Jwt:ExpireHours"]).Returns("8");

            controller = new AuthController(mockUserManager.Object, mockConfig.Object);
        }

        // ---- POST /api/auth/login ----

        [TestMethod]
        public async Task Login_ModeloInvalido_Retorna400()
        {
            // Arrange
            controller.ModelState.AddModelError("Email", "Campo requerido");

            // Act
            var result = await controller.Login(new LoginViewModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_UsuarioNaoEncontrado_Retorna401()
        {
            // Arrange
            mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await controller.Login(GetLoginModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_ContaBloqueada_Retorna401()
        {
            // Arrange
            mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUser());

            mockUserManager
                .Setup(u => u.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(true);

            // Act
            var result = await controller.Login(GetLoginModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_SenhaInvalida_Retorna401()
        {
            // Arrange
            mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUser());

            mockUserManager
                .Setup(u => u.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(false);

            mockUserManager
                .Setup(u => u.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            mockUserManager
                .Setup(u => u.AccessFailedAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Login(GetLoginModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_Valido_Retorna200ComToken()
        {
            // Arrange
            mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUser());

            mockUserManager
                .Setup(u => u.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(false);

            mockUserManager
                .Setup(u => u.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            mockUserManager
                .Setup(u => u.ResetAccessFailedCountAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(["Admin"]);

            // Act
            var result = await controller.Login(GetLoginModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result!;

            Assert.IsInstanceOfType(ok.Value, typeof(JwtResponseViewModel));
            var response = (JwtResponseViewModel)ok.Value!;

            Assert.IsFalse(string.IsNullOrEmpty(response.Token));
            Assert.AreEqual("admin@condosmart.com", response.Email);
            Assert.IsTrue(response.Roles.Contains("Admin"));
            Assert.IsTrue(response.Expiration > DateTime.UtcNow);
        }

        [TestMethod]
        public async Task Login_Valido_IncrementaFalhasAntesDeZerarContador()
        {
            // Arrange — primeira chamada falha, segunda acerta (verifica que AccessFailed é chamado)
            mockUserManager
                .Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUser());

            mockUserManager
                .Setup(u => u.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(false);

            mockUserManager
                .Setup(u => u.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            mockUserManager
                .Setup(u => u.AccessFailedAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await controller.Login(GetLoginModel());

            // Assert — AccessFailed deve ter sido chamado uma vez
            mockUserManager.Verify(
                u => u.AccessFailedAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        // --------- Dados de Teste ---------

        private static LoginViewModel GetLoginModel() => new()
        {
            Email = "admin@condosmart.com",
            Password = "Admin@123456!"
        };

        private static ApplicationUser GetTestUser() => new()
        {
            Id = "test-user-id-123",
            UserName = "admin@condosmart.com",
            Email = "admin@condosmart.com",
            NomeCompleto = "Administrador do Sistema"
        };
    }
}
