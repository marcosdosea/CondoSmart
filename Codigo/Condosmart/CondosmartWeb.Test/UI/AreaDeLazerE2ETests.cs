using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CondosmartWeb.Tests.UI
{
    [TestClass]
    public class AreaDeLazerE2ETests
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
        public void LoginTest()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);
            Assert.IsTrue(dashboardPage.IsAdminLogged(), "Admin não foi logado com sucesso");
        }

        [TestMethod]
        public void DashboardElementsTest()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);
            Assert.AreEqual("Condomínios", dashboardPage.GetCondomíniosText());
        }

        [TestMethod]
        public void CreateAreaDeLazerTest()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);
            var areasPage = dashboardPage.ClickAreasDeLazer();
            var novaAreaPage = areasPage.ClickNovaArea();
            var areasCriadaPage = novaAreaPage.CriarArea("Piscina Teste", "Piscina com deck molhado");
            areasCriadaPage.BuscarArea("Piscina");
            Assert.IsTrue(areasCriadaPage.AreaExiste("Piscina Teste"));
        }

        [TestMethod]
        public void CreateAreaDeLazer_SemNome_DeveExibirMensagemDeErro()
        {
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);

            var areasPage = dashboardPage.ClickAreasDeLazer();
            var novaAreaPage = areasPage.ClickNovaArea();

            novaAreaPage.EnterNome("");
            novaAreaPage.EnterDescricao("Teste de validação de erro.");
            novaAreaPage.ClickSalvar();

            string textErro = novaAreaPage.ObterMensagemErroNome();
            Assert.AreEqual("O campo Nome e obrigatorio", textErro);
        }

        // NOVO TESTE INTEGRADO: Tradução exata do seu segundo script Katalon
        [TestMethod]
        public void EditAreaDeLazer_CondominioInvalido_DeveExibirMensagemDeErro()
        {
            // 1. Login e Navegação
            _driver!.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login?ReturnUrl=%2F");
            var loginPage = new LoginPage(_driver);
            var dashboardPage = loginPage.Login(AdminEmail, AdminPassword);

            // 2. Acessa áreas de lazer e clica no ícone de lápis (Editar)
            var areasPage = dashboardPage.ClickAreasDeLazer();
            var editarAreaPage = areasPage.ClickEditarPrimeiraArea();

            // 3. Força a seleção do valor inválido '126942' no dropdown
            editarAreaPage.SelecionarCondominioPorValue("126942");
            editarAreaPage.ClickSalvarAlteraçoes();

            // 4. Captura o alerta de erro da tela
            string mensagemErroGlobal = editarAreaPage.ObterMensagemErroGlobal();

            // 5. Validação (Compara ignorando quebras de linha e espaços nas pontas)
            Assert.IsTrue(mensagemErroGlobal.Contains("Nao foi possivel atualizar a area de lazer agora"),
                $"Mensagem esperada não encontrada. Encontrado em tela: '{mensagemErroGlobal}'");
        }
    }
}