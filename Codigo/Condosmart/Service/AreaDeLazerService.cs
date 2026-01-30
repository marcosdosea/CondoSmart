using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados de Áreas de Lazer
    /// </summary>
    public class AreaDeLazerService : IAreaDeLazerService
    {
        private readonly CondosmartContext context;

        public AreaDeLazerService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova área de lazer na base de dados
        /// </summary>
        /// <param name="areaDeLazer">dados da área de lazer</param>
        /// <returns>id da área de lazer</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(AreaDeLazer areaDeLazer)
        {
            ValidarAreaDeLazer(areaDeLazer);

            context.Add(areaDeLazer);
            context.SaveChanges();
            return areaDeLazer.Id;
        }

        /// <summary>
        /// Editar dados da área de lazer
        /// </summary>
        /// <param name="areaDeLazer">dados da área de lazer</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(AreaDeLazer areaDeLazer)
        {
            ValidarAreaDeLazer(areaDeLazer);

            context.Update(areaDeLazer);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover a área de lazer
        /// </summary>
        /// <param name="id">id da área de lazer</param>
        public void Delete(int id)
        {
            var areaDeLazer = context.AreaDeLazer.Find(id);
            if (areaDeLazer != null)
            {
                context.Remove(areaDeLazer);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma área de lazer pelo id
        /// </summary>
        /// <param name="id">id da área de lazer</param>
        /// <returns>área de lazer encontrada ou null</returns>
        public AreaDeLazer? GetById(int id)
        {
            return context.AreaDeLazer.Find(id);
        }

        /// <summary>
        /// Buscar todas as áreas de lazer cadastradas
        /// </summary>
        /// <returns>lista de áreas de lazer</returns>
        public List<AreaDeLazer> GetAll()
        {
            return context.AreaDeLazer.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas da área de lazer
        /// </summary>
        /// <param name="areaDeLazer"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidarAreaDeLazer(AreaDeLazer areaDeLazer)
        {
            if (areaDeLazer == null)
                throw new ArgumentException("Área de lazer inválida.");

            if (string.IsNullOrWhiteSpace(areaDeLazer.Nome))
                throw new ArgumentException("O nome da área de lazer é obrigatório.");

            if (areaDeLazer.CondominioId <= 0)
                throw new ArgumentException("Condomínio inválido.");
        }
    }
}
