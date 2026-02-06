using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Serviço para gerenciar as mensalidades de condomínio
    /// </summary>
    public class MensalidadeService : IMensalidadeService
    {
        private readonly CondosmartContext context;

        public MensalidadeService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova mensalidade na base de dados
        /// </summary>
        /// <param name="mensalidade">dados da mensalidade</param>
        /// <returns>id da mensalidade</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Mensalidade mensalidade)
        {
            ValidarMensalidade(mensalidade);

            context.Add(mensalidade);
            context.SaveChanges();
            return mensalidade.Id;
        }

        /// <summary>
        /// Editar dados da mensalidade na base de dados
        /// </summary>
        /// <param name="mensalidade">dados da mensalidade</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Mensalidade mensalidade)
        {
            ValidarMensalidade(mensalidade);

            context.Update(mensalidade);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover a mensalidade da base de dados
        /// </summary>
        /// <param name="id">identificador da mensalidade</param>
        public void Delete(int id)
        {
            var mensalidade = GetById(id);
            if (mensalidade != null)
            {
                context.Remove(mensalidade);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Obter uma mensalidade pelo seu identificador
        /// </summary>
        /// <param name="id">identificador da mensalidade</param>
        /// <returns>dados da mensalidade ou nulo</returns>
        public Mensalidade? GetById(int id)
        {
            return context.Mensalidades?
                .Include(m => m.Condominio)
                .Include(m => m.Morador)
                .Include(m => m.Unidade)
                .Include(m => m.Pagamento)
                .FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Obter a lista com todas as mensalidades
        /// </summary>
        /// <returns>lista de mensalidades</returns>
        public List<Mensalidade> GetAll()
        {
            return context.Mensalidades?
                .Include(m => m.Condominio)
                .Include(m => m.Morador)
                .Include(m => m.Unidade)
                .Include(m => m.Pagamento)
                .ToList() ?? new List<Mensalidade>();
        }

        /// <summary>
        /// Validar os dados da mensalidade
        /// </summary>
        /// <param name="mensalidade">dados da mensalidade</param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarMensalidade(Mensalidade mensalidade)
        {
            if (mensalidade == null)
                throw new ArgumentException("A mensalidade não pode ser nula");

            if (mensalidade.Valor <= 0)
                throw new ArgumentException("O valor da mensalidade deve ser maior que zero");

            if (mensalidade.UnidadeId <= 0)
                throw new ArgumentException("O identificador da unidade é obrigatório");

            if (mensalidade.CondominioId <= 0)
                throw new ArgumentException("O identificador do condomínio é obrigatório");

            if (string.IsNullOrWhiteSpace(mensalidade.Status))
                throw new ArgumentException("O status é obrigatório");

            if (mensalidade.Competencia == default)
                throw new ArgumentException("A competência é obrigatória");

            if (mensalidade.Vencimento == default)
                throw new ArgumentException("A data de vencimento é obrigatória");
        }
    }
}
