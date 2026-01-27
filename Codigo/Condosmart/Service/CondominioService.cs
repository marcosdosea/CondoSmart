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
            ValidarCondominio(condominio);

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
            ValidarCondominio(condominio);

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
        private static void ValidarCondominio(Condominio condominio)
        {
            if (condominio == null)
                throw new ArgumentException("Condomínio inválido.");

            if (string.IsNullOrWhiteSpace(condominio.Nome))
                throw new ArgumentException("O nome do condomínio é obrigatório.");

            // CNPJ no banco está char(14) pelo scaffold, então esperamos 14 dígitos
            if (!string.IsNullOrWhiteSpace(condominio.Cnpj) && condominio.Cnpj.Length != 14)
                throw new ArgumentException("O CNPJ deve conter 14 caracteres (somente números).");

            if (!string.IsNullOrWhiteSpace(condominio.Uf) && condominio.Uf.Length != 2)
                throw new ArgumentException("UF deve ter 2 caracteres.");

            if (!string.IsNullOrWhiteSpace(condominio.Cep) && condominio.Cep.Length != 8)
                throw new ArgumentException("CEP deve ter 8 caracteres (somente números).");

            if (condominio.Unidades < 0)
                throw new ArgumentException("Quantidade de unidades não pode ser negativa.");
        }
    }
}
