using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do síndico
    /// </summary>
    public class SindicoService : ISindicoService
    {
        private readonly CondosmartContext context;

        public SindicoService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo síndico na base de dados
        /// </summary>
        /// <param name="sindico">dados do síndico</param>
        /// <returns>id do síndico</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Sindico sindico)
        {
            ValidarSindico(sindico);

            context.Add(sindico);
            context.SaveChanges();
            return sindico.Id;
        }

        /// <summary>
        /// Editar dados do síndico na base de dados
        /// </summary>
        /// <param name="sindico">dados do síndico</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Sindico sindico)
        {
            ValidarSindico(sindico);

            context.Update(sindico);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover o síndico da base de dados
        /// </summary>
        /// <param name="id">id do síndico</param>
        public void Delete(int id)
        {
            var sindico = context.Sindicos.Find(id);
            if (sindico != null)
            {
                context.Remove(sindico);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um síndico na base de dados
        /// </summary>
        /// <param name="id">id do síndico</param>
        /// <returns>dados do síndico</returns>
        public Sindico? GetById(int id)
        {
            return context.Sindicos.Find(id);
        }

        /// <summary>
        /// Buscar todos os síndicos cadastrados
        /// </summary>
        /// <returns>lista de síndicos</returns>
        public List<Sindico> GetAll()
        {
            return context.Sindicos.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas do síndico
        /// </summary>
        /// <param name="sindico"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidarSindico(Sindico sindico)
        {
            if (sindico == null)
                throw new ArgumentException("Síndico inválido.");

            if (string.IsNullOrWhiteSpace(sindico.Nome))
                throw new ArgumentException("O nome do síndico é obrigatório.");

            // CPF geralmente vem como char(11) no banco
            if (!string.IsNullOrWhiteSpace(sindico.Cpf) && sindico.Cpf.Length != 11)
                throw new ArgumentException("O CPF deve conter 11 caracteres (somente números).");

            if (!string.IsNullOrWhiteSpace(sindico.Uf) && sindico.Uf.Length != 2)
                throw new ArgumentException("UF deve ter 2 caracteres.");

            if (!string.IsNullOrWhiteSpace(sindico.Cep) && sindico.Cep.Length != 8)
                throw new ArgumentException("CEP deve ter 8 caracteres (somente números).");
        }
    }
}
