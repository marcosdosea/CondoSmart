using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do morador
    /// </summary>
    public class MoradorService : IMoradorService
    {
        private readonly CondosmartContext context;

        public MoradorService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo morador na base de dados
        /// </summary>
        /// <param name="morador">dados do morador</param>
        /// <returns>id do morador</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Morador morador)
        {
            // ValidarMorador(morador); // Removido: validação feita na ViewModel

            context.Add(morador);
            context.SaveChanges();
            return morador.Id;
        }

        /// <summary>
        /// Editar dados do morador na base de dados
        /// </summary>
        /// <param name="morador">dados do morador</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Morador morador)
        {
            // ValidarMorador(morador); // Removido: validação feita na ViewModel

            context.Update(morador);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover o morador da base de dados
        /// </summary>
        /// <param name="id">id do morador</param>
        public void Delete(int id)
        {
            var morador = context.Moradores.Find(id);
            if (morador != null)
            {
                context.Remove(morador);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um morador na base de dados
        /// </summary>
        /// <param name="id">id do morador</param>
        /// <returns>dados do morador</returns>
        public Morador? GetById(int id)
        {
            return context.Moradores.Find(id);
        }

        /// <summary>
        /// Buscar todos os moradores cadastrados
        /// </summary>
        /// <returns>lista de moradores</returns>
        public List<Morador> GetAll()
        {
            return context.Moradores.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas do morador
        /// </summary>
        /// <param name="morador"></param>
        /// <exception cref="ArgumentException"></exception>
        /* 
        private static void ValidarMorador(Morador morador)
        {
            if (morador == null)
                throw new ServiceException("Morador inválido.");
            // Outras validações agora são tratadas na MoradorViewModel
        }
        */
    }
}
