using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados das unidades residenciais
    /// (substitui a implementação anterior de condomínio neste arquivo
    /// para suportar IUnidadesResidenciaisService).
    /// </summary>
    public class UnidadesResidenciaisService : IUnidadesResidenciaisService
    {
        private readonly CondosmartContext context;

        public UnidadesResidenciaisService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova unidade residencial na base de dados
        /// </summary>
        /// <param name="unidade">dados da unidade</param>
        /// <returns>id da unidade</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(UnidadesResidenciais unidade)
        {
            // ValidarUnidade(unidade); // Removido - agora na ViewModel e ServiceException se necessário

            context.UnidadesResidenciais.Add(unidade);
            context.SaveChanges();
            return unidade.Id;
        }

        /// <summary>
        /// Editar dados da unidade residencial na base de dados
        /// </summary>
        /// <param name="unidade">dados da unidade</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(UnidadesResidenciais unidade)
        {
            // ValidarUnidade(unidade);

            context.UnidadesResidenciais.Update(unidade);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover a unidade residencial da base de dados
        /// </summary>
        /// <param name="id">id da unidade</param>
        public void Delete(int id)
        {
            var unidade = context.UnidadesResidenciais.Find(id);
            if (unidade != null)
            {
                context.UnidadesResidenciais.Remove(unidade);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma unidade residencial na base de dados (inclui referências básicas)
        /// </summary>
        /// <param name="id">id da unidade</param>
        /// <returns>dados da unidade</returns>
        public UnidadesResidenciais? GetById(int id)
        {
            return context.UnidadesResidenciais
                          .AsNoTracking()
                          .Include(u => u.Morador)
                          .Include(u => u.Condominio)
                          .FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Buscar todas as unidades residenciais cadastradas
        /// </summary>
        /// <returns>lista de unidades</returns>
        public List<UnidadesResidenciais> GetAll()
        {
            return context.UnidadesResidenciais
                          .AsNoTracking()
                          .Include(u => u.Morador)
                          .Include(u => u.Condominio)
                          .ToList();
        }

        /// <summary>
        /// Valida regras básicas da unidade residencial
        /// </summary>
        /// <param name="unidade"></param>
        /// <exception cref="ArgumentException"></exception>
        // ValidarUnidade removido conforme refatoração para ViewModel e ServiceException
    }
}