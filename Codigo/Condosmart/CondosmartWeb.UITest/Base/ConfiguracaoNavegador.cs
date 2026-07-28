using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace CondosmartWeb.UITest.Base
{
    [TestClass]
    public abstract class ConfiguracaoNavegador
    {
        protected IWebDriver Driver { get; private set; }

        // Mude para a URL e porta real em que a aplicação roda no seu ambiente local (ex: IIS Express ou Kestrel)
        protected const string BaseUrl = "https://localhost:7290"; 

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            // Descomente a linha abaixo para rodar sem abrir a janela do navegador (Headless) em ambientes de CI/CD
            // options.AddArgument("--headless"); 
            options.AddArgument("--start-maximized");
            
            // O Selenium Manager no Selenium 4+ gerencia automaticamente o ChromeDriver
            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }
        }
    }
}
