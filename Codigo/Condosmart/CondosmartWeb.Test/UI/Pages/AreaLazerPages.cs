using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CondosmartWeb.Tests.UI
{
    public class LoginPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By EmailInput => By.CssSelector("input[type='email']");
        private By PasswordInput => By.CssSelector("input[type='password']");
        private By LoginButton => By.CssSelector("button[type='submit']");

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterEmail(string email)
        {
            _wait.Until(d => d.FindElement(EmailInput).Displayed);
            _driver.FindElement(EmailInput).SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            _wait.Until(d => d.FindElement(PasswordInput).Displayed);
            _driver.FindElement(PasswordInput).SendKeys(password);
        }

        public void ClickLoginButton()
        {
            _wait.Until(d => d.FindElement(LoginButton).Enabled);
            _driver.FindElement(LoginButton).Click();
        }

        public DashboardPage Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLoginButton();
            return new DashboardPage(_driver);
        }
    }

    public class DashboardPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By AdminSpan => By.XPath("//span[contains(text(), 'Administrador')]");
        private By CondomíniosLink => By.LinkText("Condomínios");
        private By AreasLazerLink => By.XPath("//a[contains(normalize-space(), 'reas de Lazer')]");

        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public bool IsAdminLogged()
        {
            try
            {
                return _wait.Until(d => d.FindElement(AdminSpan).Displayed);
            }
            catch
            {
                return false;
            }
        }

        public string GetCondomíniosText()
        {
            return _wait.Until(d => d.FindElement(CondomíniosLink)).Text;
        }

        public AreasDeLazerPage ClickAreasDeLazer()
        {
            var elemento = _wait.Until(d => d.FindElement(AreasLazerLink));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", elemento);

            return new AreasDeLazerPage(_driver);
        }
    }

    public class AreasDeLazerPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By NovaAreaButton => By.XPath("//a[contains(normalize-space(), 'Nova')] | //a[contains(@href, 'Create')] | //button[contains(normalize-space(), 'Nova')]");
        private By BuscaInput => By.CssSelector("input[type='search'], input.form-control-sm, input[type='text']");
        
        // Tradução do 'Page_reas de Lazer - CondoSmart/i_bi bi-pencil' (Botão de Editar da Tabela)
        private By EditarButton => By.CssSelector("i.bi-pencil, a[href*='Edit']");

        public AreasDeLazerPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public NovaAreaDeLazerPage ClickNovaArea()
        {
            var elemento = _wait.Until(d => d.FindElement(NovaAreaButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", elemento);

            return new NovaAreaDeLazerPage(_driver);
        }

        // NOVO MÉTODO: Clica no primeiro ícone de lápis/editar da listagem
        public EditarAreaDeLazerPage ClickEditarPrimeiraArea()
        {
            var elemento = _wait.Until(d => d.FindElement(EditarButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", elemento);

            return new EditarAreaDeLazerPage(_driver);
        }

        public void BuscarArea(string termo)
        {
            var input = _wait.Until(d => d.FindElement(BuscaInput));
            input.Clear();
            input.SendKeys(termo);
        }

        public bool AreaExiste(string nome)
        {
            try
            {
                return _wait.Until(d => d.FindElement(By.XPath($"//td[contains(text(), '{nome}')]")).Displayed);
            }
            catch
            {
                return false;
            }
        }
    }

    public class NovaAreaDeLazerPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        private By NomeInput => By.Id("Nome");
        private By DescricaoTextarea => By.Id("Descricao");
        private By SalvarButton => By.XPath("//button[contains(normalize-space(), 'Salvar')] | //input[@type='submit']");
        private By NomeErrorSpan => By.Id("Nome-error");

        public NovaAreaDeLazerPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterNome(string nome)
        {
            var input = _wait.Until(d => {
                var el = d.FindElement(NomeInput);
                return (el.Displayed && el.Enabled) ? el : null;
            });
            input.Clear();
            input.SendKeys(nome);
        }

        public void EnterDescricao(string descricao)
        {
            var textarea = _wait.Until(d => {
                try { return d.FindElement(DescricaoTextarea); }
                catch { return d.FindElement(By.CssSelector("textarea")); }
            });
            textarea.Clear();
            textarea.SendKeys(descricao);
        }

        public void ClickSalvar()
        {
            var botao = _wait.Until(d => d.FindElement(SalvarButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botao);
        }

        public AreasDeLazerPage CriarArea(string nome, string descricao)
        {
            EnterNome(nome);
            EnterDescricao(descricao);
            ClickSalvar();

            System.Threading.Thread.Sleep(1500);

            return new AreasDeLazerPage(_driver);
        }

        public string ObterMensagemErroNome()
        {
            return _wait.Until(d => {
                var el = d.FindElement(NomeErrorSpan);
                return el.Displayed ? el.Text : string.Empty;
            });
        }
    }

    // NOVA CLASSE PAGE OBJECT: Responsável pela tela de Edição
    public class EditarAreaDeLazerPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        // Elementos baseados no seu segundo script do Katalon
        private By CondominioSelect => By.CssSelector("select[name*='Condominio'], select, #CondominioId");
        private By SalvarAlteracoesButton => By.XPath("//button[contains(normalize-space(), 'Salvar')] | //input[@type='submit']");
        private By AlertaErroDiv => By.CssSelector(".alert-danger, div[class*='ValidationSummary'], form div.text-danger");

        public EditarAreaDeLazerPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void SelecionarCondominioPorValue(string value)
        {
            var selectElement = _wait.Until(d => d.FindElement(CondominioSelect));
            var selectObject = new SelectElement(selectElement);
            selectObject.SelectByValue(value);
        }

        public void ClickSalvarAlteraçoes()
        {
            var botao = _wait.Until(d => d.FindElement(SalvarAlteracoesButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botao);
        }

        public string ObterMensagemErroGlobal()
        {
            return _wait.Until(d => {
                var el = d.FindElement(AlertaErroDiv);
                return el.Displayed ? el.Text.Trim() : string.Empty;
            });
        }
    }
}