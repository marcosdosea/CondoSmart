using CondosmartWeb.UITest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace CondosmartWeb.UITest.Tests
{
    [TestClass]
    public class ExemploHomeTest : ConfiguracaoNavegador
    {
        [TestMethod]
        public void AcessarPaginaInicial_DeveCarregarComSucesso()
        {
            // Arrange & Act
            // Navega para a URL base da aplicação
            Driver.Navigate().GoToUrl(BaseUrl);

            // Assert
            // Verifica se o título da página ou alguma tag principal carregou
            // Exemplo: var titulo = Driver.Title;
            // Assert.IsNotNull(titulo);
            
            // Aqui estamos verificando se a página não retorna um erro
            Assert.IsTrue(Driver.PageSource.Contains("html", System.StringComparison.OrdinalIgnoreCase), "O HTML da página não foi carregado.");
        }
    }
}
