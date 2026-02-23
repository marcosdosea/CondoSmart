using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;
using Service;
using Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CondosmartWeb.Tests.Services
{
    [TestClass]
    public class ReservaServiceTests
    {
        private CondosmartContext _context = null!;
        private ReservaService _service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CondosmartContext>()
                .UseInMemoryDatabase(databaseName: "CondosmartTest")
                .Options;

            _context = new CondosmartContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _service = new ReservaService(_context);
        }

        [TestMethod]
        public void Create_DataFimMenorQueInicio_ThrowsServiceException()
        {
            // Arrange
            var reserva = new Reserva
            {
                DataInicio = DateTime.Now.AddDays(1),
                DataFim = DateTime.Now, // Fim antes do Início
                Status = "pendente",
                AreaId = 1,
                CondominioId = 1
            };

            // Act & Assert
            Assert.ThrowsException<ServiceException>(() => _service.Create(reserva));
        }

        [TestMethod]
        public void Create_ReservaValida_RetornaId()
        {
            // Arrange
            var reserva = new Reserva
            {
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddHours(2),
                Status = "pendente",
                AreaId = 1,
                CondominioId = 1
            };

            // Act
            var id = _service.Create(reserva);

            // Assert
            Assert.IsTrue(id > 0);
            var salva = _context.Reservas.Find(id);
            Assert.IsNotNull(salva);
            Assert.AreEqual("pendente", salva.Status);
        }
    }
}
