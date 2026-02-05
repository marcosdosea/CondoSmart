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
            ValidarMorador(morador);

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
            ValidarMorador(morador);

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
        private static void ValidarMorador(Morador morador)
        {
            if (morador == null)
                throw new ArgumentException("Morador inválido.");

            if (string.IsNullOrWhiteSpace(morador.Nome))
                throw new ArgumentException("O nome do morador é obrigatório.");

            if (string.IsNullOrWhiteSpace(morador.Cpf))
                throw new ArgumentException("O CPF do morador é obrigatório.");

            if (!string.IsNullOrWhiteSpace(morador.Cpf) && morador.Cpf.Length != 11)
                throw new ArgumentException("O CPF deve conter 11 caracteres (somente números).");

            if (!string.IsNullOrWhiteSpace(morador.Cep) && morador.Cep.Length != 8)
                throw new ArgumentException("CEP deve ter 8 caracteres (somente números).");

            if (!string.IsNullOrWhiteSpace(morador.Uf) && morador.Uf.Length != 2)
                throw new ArgumentException("UF deve ter 2 caracteres.");
        }
    }
}
