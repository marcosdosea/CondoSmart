using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CondosmartWeb.Tests.UI
{
    public class VisitanteIndexPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By PageHeading => By.XPath("//h2[contains(text(), 'Visitantes')]");
        private By NovoVisitanteButton => By.XPath("//a[contains(@href, 'Visitante/Create')]");

        public VisitanteIndexPage(IWebDriver driver)
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

        public bool BotaoNovoVisitanteVisivel()
        {
            try
            {
                return _wait.Until(d => d.FindElement(NovoVisitanteButton).Displayed);
            }
            catch
            {
                return false;
            }
        }
    }
}
