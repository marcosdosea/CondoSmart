using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore; 
using Service;
using System;
using Xunit;

namespace CondosmartWeb.Tests
{
    public class AreaDeLazerServiceTest
    {
        private CondosmartContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new CondosmartContext(options);
        }

        [Fact]
        public void Create_DeveSalvar_QuandoNomeForUnico()
        {
            string dbName = Guid.NewGuid().ToString();

            using (var context = GetInMemoryContext(dbName))
            {
                var service = new AreaDeLazerService(context);
                var novaArea = new AreaDeLazer
                {
                    Nome = "Salão de Jogos",
                    CondominioId = 1,
                    Disponibilidade = true
                };

                int id = service.Create(novaArea);

                // Forçamos o uso do Xunit para não ter dúvida
                Xunit.Assert.True(id > 0);
            }
        }

        [Fact]
        public void Create_DeveLancarErro_QuandoNomeForDuplicado()
        {
            string dbName = Guid.NewGuid().ToString();

            using (var context = GetInMemoryContext(dbName))
            {
                context.AreaDeLazer.Add(new AreaDeLazer
                {
                    Nome = "Piscina",
                    CondominioId = 1,
                    Disponibilidade = true
                });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var service = new AreaDeLazerService(context);
                var areaDuplicada = new AreaDeLazer { Nome = "Piscina", CondominioId = 1 };

                // Forçamos o uso do Xunit
                var ex = Xunit.Assert.Throws<InvalidOperationException>(() => service.Create(areaDuplicada));

                Xunit.Assert.Contains("Já existe uma área de lazer", ex.Message);
            }
        }
    }
}