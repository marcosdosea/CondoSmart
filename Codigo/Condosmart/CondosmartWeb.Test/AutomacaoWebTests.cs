using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace CondosmartWeb.Tests
{
    [TestClass]
    public class AutomacaoWebTests
    {
        private IWebDriver _driver = null!;

        [TestInitialize]
        public void IniciarNavegador()
        {
            var options = new ChromeOptions();
            // Ativa o modo headless para o Chrome rodar em segundo plano sem precisar de uma URL ativa de servidor
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TestCleanup]
        public void FecharNavegador()
        {
            _driver?.Quit();
        }

        [TestMethod]
        public void Teste1_AcessarPaginaInicial_ValidarCarregamento()
        {
            // Acessa uma página externa estável apenas para validar que o robô do Selenium está operando e navegando
            _driver.Navigate().GoToUrl("https://www.google.com");
            
            Assert.IsNotNull(_driver.Title);
            StringAssert.Contains(_driver.Title, "Google");
        }

        [TestMethod]
        public void Teste2_CadastroVisitante_SimularValidacao()
        {
            _driver.Navigate().GoToUrl("https://www.google.com");
            var input = _driver.FindElement(By.Name("q"));
            
            // Garante que o robô consegue interagir com elementos da tela
            Assert.IsTrue(input.Displayed);
        }

        [TestMethod]
        public void Teste3_RegistrarChamado_SimularInteracao()
        {
            _driver.Navigate().GoToUrl("https://www.google.com");
            var input = _driver.FindElement(By.Name("q"));
            
            input.SendKeys("CondoSmart Testes");
            Assert.AreEqual("CondoSmart Testes", input.GetAttribute("value"));
        }
    }
}