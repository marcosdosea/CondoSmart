using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using System.Linq;

namespace CondosmartWeb.Tests.Service
{
    [TestClass]
    public class NotificacaoServiceTests
    {
        private CondosmartContext context = null!;
        private NotificacaoService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            context = new CondosmartContext(options);
            service = new NotificacaoService(context);

            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Dispose();
        }

        [TestMethod]
        public void CriarTest_ComDadosValidos_AdicionaNoBancoDeDados()
        {
            // Arrange
            var countAntes = context.NotificacoesSistema.Count();

            // Act
            service.Criar(
                usuarioEmail: "teste@condosmart.com",
                usuarioNome: "Usuário Teste",
                titulo: "Nova Notificação",
                mensagem: "Esta é uma mensagem de teste.",
                tipo: "info",
                condominioId: 1
            );

            // Assert
            var countDepois = context.NotificacoesSistema.Count();
            Assert.AreEqual(countAntes + 1, countDepois);

            var notificacaoCriada = context.NotificacoesSistema.Last();
            Assert.AreEqual("Nova Notificação", notificacaoCriada.Titulo);
            Assert.AreEqual("teste@condosmart.com", notificacaoCriada.UsuarioEmail);
            Assert.AreEqual(1, notificacaoCriada.CondominioId);
        }

        [TestMethod]
        public void ContarTest_ComCondominioId_RetornaQuantidadeCorreta()
        {
            // Act
            var totalCondominio1 = service.Contar(1);
            var totalCondominio2 = service.Contar(2);
            var totalSemFiltro = service.Contar(null);

            // Assert
            Assert.AreEqual(2, totalCondominio1, "O condomínio 1 deve ter 2 notificações.");
            Assert.AreEqual(1, totalCondominio2, "O condomínio 2 deve ter 1 notificação.");
            Assert.AreEqual(3, totalSemFiltro, "O total geral sem filtro deve ser 3.");
        }

        [TestMethod]
        public void RemoverTest_ComIdExistente_RemoveDoBancoDeDados()
        {
            // Arrange
            var idParaRemover = 1;
            var existeAntes = context.NotificacoesSistema.Any(n => n.Id == idParaRemover);
            Assert.IsTrue(existeAntes, "A notificação deve existir antes da remoção.");

            // Act
            service.Remover(idParaRemover);

            // Assert
            var existeDepois = context.NotificacoesSistema.Any(n => n.Id == idParaRemover);
            Assert.IsFalse(existeDepois, "A notificação não deve existir após a remoção.");
        }

        private void SeedTestData()
        {
            var n1 = new NotificacaoSistema
            {
                Id = 1,
                UsuarioEmail = "sindico@condosmart.com",
                UsuarioNome = "Síndico",
                Titulo = "Aviso 1",
                Mensagem = "Mensagem 1",
                Tipo = "info",
                CondominioId = 1,
                CreatedAt = DateTime.Now
            };

            var n2 = new NotificacaoSistema
            {
                Id = 2,
                UsuarioEmail = "morador1@condosmart.com",
                UsuarioNome = "Morador 1",
                Titulo = "Aviso 2",
                Mensagem = "Mensagem 2",
                Tipo = "warning",
                CondominioId = 1,
                CreatedAt = DateTime.Now
            };

            var n3 = new NotificacaoSistema
            {
                Id = 3,
                UsuarioEmail = "morador2@condosmart.com",
                UsuarioNome = "Morador 2",
                Titulo = "Aviso 3",
                Mensagem = "Mensagem 3",
                Tipo = "danger",
                CondominioId = 2,
                CreatedAt = DateTime.Now
            };

            context.NotificacoesSistema.AddRange(n1, n2, n3);
            context.SaveChanges();
        }
    }
}
