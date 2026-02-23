using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do condomínio
    /// </summary>
    public class CondominioService : ICondominioService
    {
        private readonly CondosmartContext context;

        public CondominioService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo condomínio na base de dados
        /// </summary>
        /// <param name="condominio">dados do condomínio</param>
        /// <returns>id do condomínio</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Condominio condominio)
        {
            // ValidarCondominio(condominio); // Removido: validação feita na ViewModel

            context.Add(condominio);
            context.SaveChanges();
            return condominio.Id;
        }

        /// <summary>
        /// Editar dados do condomínio na base de dados
        /// </summary>
        /// <param name="condominio">dados do condomínio</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Condominio condominio)
        {
            // ValidarCondominio(condominio); // Removido: validação feita na ViewModel

            context.Update(condominio);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover o condomínio da base de dados
        /// </summary>
        /// <param name="id">id do condomínio</param>
        public void Delete(int id)
        {
            var condominio = context.Condominios.Find(id);
            if (condominio != null)
            {
                context.Remove(condominio);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um condomínio na base de dados
        /// </summary>
        /// <param name="id">id do condomínio</param>
        /// <returns>dados do condomínio</returns>
        public Condominio? GetById(int id)
        {
            return context.Condominios.Find(id);
        }

        /// <summary>
        /// Buscar todos os condomínios cadastrados
        /// </summary>
        /// <returns>lista de condomínios</returns>
        public List<Condominio> GetAll()
        {
            return context.Condominios.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas do condomínio (modelo parecido com o do professor)
        /// </summary>
        /// <param name="condominio"></param>
        /// <exception cref="ArgumentException"></exception>
        /*
        private static void ValidarCondominio(Condominio condominio)
        {
            if (condominio == null)
                throw new Core.Exceptions.ServiceException("Condomínio inválido.");

            if (condominio.Unidades < 0)
                throw new Core.Exceptions.ServiceException("Quantidade de unidades não pode ser negativa.");
        }
        */
    }
}
