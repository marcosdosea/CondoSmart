using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do visitante
    /// </summary>
    public class VisitanteService : IVisitanteService
    {
        private readonly CondosmartContext context;

        public VisitanteService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo visitante na base de dados
        /// </summary>
        /// <param name="visitante">dados do visitante</param>
        /// <returns>id do visitante</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Visitantes visitante)
        {
            ValidarVisitante(visitante);

            context.Add(visitante);
            context.SaveChanges();
            return visitante.Id;
        }

        /// <summary>
        /// Editar dados do visitante na base de dados
        /// </summary>
        /// <param name="visitante">dados do visitante</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Visitantes visitante)
        {
            ValidarVisitante(visitante);

            context.Update(visitante);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover o visitante da base de dados
        /// </summary>
        /// <param name="id">id do visitante</param>
        public void Delete(int id)
        {
            var visitante = context.Visitantes.Find(id);
            if (visitante != null)
            {
                context.Remove(visitante);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um visitante na base de dados
        /// </summary>
        /// <param name="id">id do visitante</param>
        /// <returns>dados do visitante</returns>
        public Visitantes? GetById(int id)
        {
            return context.Visitantes.Find(id);
        }

        /// <summary>
        /// Buscar todos os visitantes cadastrados
        /// </summary>
        /// <returns>lista de visitantes</returns>
        public List<Visitantes> GetAll()
        {
            return context.Visitantes.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas do visitante
        /// </summary>
        /// <param name="visitante"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidarVisitante(Visitantes visitante)
        {
            if (visitante == null)
                throw new ArgumentException("Visitante inválido.");

            if (string.IsNullOrWhiteSpace(visitante.Nome))
                throw new ArgumentException("O nome do visitante é obrigatório.");

            // CPF geralmente é char(11) no banco
            if (!string.IsNullOrWhiteSpace(visitante.Cpf) && visitante.Cpf.Length != 11)
                throw new ArgumentException("O CPF deve conter 11 caracteres (somente números).");

            // Regras simples de horário (opcional, mas ajuda)
            if (visitante.DataHoraSaida.HasValue && visitante.DataHoraEntrada.HasValue &&
                visitante.DataHoraSaida.Value < visitante.DataHoraEntrada.Value)
            {
                throw new ArgumentException("A data/hora de saída não pode ser menor que a entrada.");
            }
        }
    }
}
