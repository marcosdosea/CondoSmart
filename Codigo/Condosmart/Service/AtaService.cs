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
    /// Implementa serviços para manter dados de Atas
    /// </summary>
    public class AtaService : IAtaService
    {
        private readonly CondosmartContext context;

        public AtaService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova ata na base de dados
        /// </summary>
        /// <param name="ata">dados da ata</param>
        /// <returns>id da ata</returns>
        public int Create(Ata ata)
        {
            // ValidarAta(ata); // Removido - agora na ViewModel e ServiceException se necessário

            context.Add(ata);
            context.SaveChanges();
            return ata.Id;
        }

        /// <summary>
        /// Editar dados da ata
        /// </summary>
        /// <param name="ata">dados da ata</param>
        public void Edit(Ata ata)
        {
            // ValidarAta(ata);

            context.Update(ata);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover a ata
        /// </summary>
        /// <param name="id">id da ata</param>
        public void Delete(int id)
        {
            var ata = context.Atas.Find(id);
            if (ata != null)
            {
                context.Remove(ata);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma ata pelo id
        /// </summary>
        /// <param name="id">id da ata</param>
        /// <returns>ata encontrada ou null</returns>
        public Ata? GetById(int id)
        {
            return context.Atas.Find(id);
        }

        /// <summary>
        /// Buscar todas as atas cadastradas
        /// </summary>
        /// <returns>lista de atas</returns>
        public List<Ata> GetAll()
        {
            return context.Atas.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas da ata
        /// </summary>
        /// <param name="ata"></param>
        // ValidarAta removido conforme refatoração para ViewModel e ServiceException
    }
}
