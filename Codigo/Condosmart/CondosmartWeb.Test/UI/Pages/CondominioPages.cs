using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CondosmartWeb.Tests.UI
{
    public class CondominioIndexPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By PageHeading => By.XPath("//h2[contains(text(), 'Condom')]");
        private By NovoCondominioButton => By.XPath("//a[contains(@href, 'Condominio/Create')]");
        private By TabelaCondominios => By.CssSelector("table");

        public CondominioIndexPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public bool PaginaCarregada()
        {
            try
            {
                return _wait.Until(d => d.FindElement(PageHeading).Displayed);
            }
            catch
            {
                return false;
            }
        }

        public bool BotaoNovoCondominioVisivel()
        {
            try
            {
                return _wait.Until(d => d.FindElement(NovoCondominioButton).Displayed);
            }
            catch
            {
                return false;
            }
        }

        public CondominioCreatePage ClickNovoCondominio()
        {
            var elemento = _wait.Until(d => d.FindElement(NovoCondominioButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", elemento);
            return new CondominioCreatePage(_driver);
        }
    }

    public class CondominioCreatePage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By NomeInput => By.Id("Nome");
        private By NomeErrorSpan => By.Id("Nome-error");
        private By SalvarButton => By.XPath("//button[contains(normalize-space(), 'Salvar')]");

        public CondominioCreatePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void ClickSalvar()
        {
            var botao = _wait.Until(d => d.FindElement(SalvarButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botao);
        }

        public string ObterMensagemErroNome()
        {
            return _wait.Until(d =>
            {
                var el = d.FindElement(NomeErrorSpan);
                return el.Displayed ? el.Text : string.Empty;
            });
        }
    }
}
