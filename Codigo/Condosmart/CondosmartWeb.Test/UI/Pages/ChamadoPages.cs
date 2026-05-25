using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CondosmartWeb.Tests.UI
{
    public class ChamadoIndexPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By PageHeading => By.XPath("//h2[contains(text(), 'Chamados')]");
        private By NovoChamadoButton => By.XPath("//a[contains(@href, 'Chamado/Create')]");

        public ChamadoIndexPage(IWebDriver driver)
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

        public ChamadoCreatePage ClickNovoChamado()
        {
            var elemento = _wait.Until(d => d.FindElement(NovoChamadoButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", elemento);
            return new ChamadoCreatePage(_driver);
        }
    }

    public class ChamadoCreatePage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By DescricaoTextarea => By.Id("Descricao");
        private By DescricaoErrorSpan => By.Id("Descricao-error");
        private By SalvarButton => By.XPath("//button[contains(normalize-space(), 'Salvar')]");

        public ChamadoCreatePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void LimparDescricao()
        {
            var textarea = _wait.Until(d => d.FindElement(DescricaoTextarea));
            textarea.Clear();
        }

        public void ClickSalvar()
        {
            var botao = _wait.Until(d => d.FindElement(SalvarButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botao);
        }

        public string ObterMensagemErroDescricao()
        {
            return _wait.Until(d =>
            {
                var el = d.FindElement(DescricaoErrorSpan);
                return el.Displayed ? el.Text : string.Empty;
            });
        }
    }
}
