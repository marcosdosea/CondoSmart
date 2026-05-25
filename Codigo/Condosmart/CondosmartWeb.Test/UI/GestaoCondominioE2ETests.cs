using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CondosmartWeb.Tests.UI
{
    [TestClass]
    public class GestaoCondominioE2ETests
    {
        private IWebDriver? _driver;
        private const string BaseUrl = "https://localhost:7290";
        private const string AdminEmail = "admin@condosmart.com";
        private const string AdminPassword = "Admin@123";

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AcceptInsecureCertificates = true;
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver?.Quit();
        }

        [TestMethod]
        public void CondominioIndex_DeveCarregarPaginaComBotaoNovo()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);

            _driver.Navigate().GoToUrl($"{BaseUrl}/Condominio");
            var condominioPage = new CondominioIndexPage(_driver);

            Assert.IsTrue(condominioPage.PaginaCarregada(), "A página de Condomínios não carregou corretamente");
            Assert.IsTrue(condominioPage.BotaoNovoCondominioVisivel(), "O botão 'Novo Condomínio' não está visível");
        }

        [TestMethod]
        public void ChamadoCreate_SemDescricao_DeveExibirMensagemDeErro()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);

            _driver.Navigate().GoToUrl($"{BaseUrl}/Chamado/Create");
            var chamadoCreatePage = new ChamadoCreatePage(_driver);

            chamadoCreatePage.LimparDescricao();
            chamadoCreatePage.ClickSalvar();

            string mensagemErro = chamadoCreatePage.ObterMensagemErroDescricao();
            Assert.IsFalse(string.IsNullOrEmpty(mensagemErro), "A mensagem de erro de validação da descrição não foi exibida");
        }

        [TestMethod]
        public void VisitanteIndex_DeveCarregarPaginaComBotaoNovo()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);

            _driver.Navigate().GoToUrl($"{BaseUrl}/Visitante");
            var visitantePage = new VisitanteIndexPage(_driver);

            Assert.IsTrue(visitantePage.PaginaCarregada(), "A página de Visitantes não carregou corretamente");
            Assert.IsTrue(visitantePage.BotaoNovoVisitanteVisivel(), "O botão 'Novo Visitante' não está visível");
        }
    }
}
