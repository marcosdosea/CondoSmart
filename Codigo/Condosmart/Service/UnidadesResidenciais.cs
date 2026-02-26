using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

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
            ValidarUnidade(unidade);

            bool duplicado = context.UnidadesResidenciais.Any(u =>
                u.CondominioId == unidade.CondominioId &&
                u.Identificador == unidade.Identificador);

            if (duplicado)
                throw new ArgumentException($"Já existe uma unidade com o identificador '{unidade.Identificador}' neste condomínio.");

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
            ValidarUnidade(unidade);

            bool duplicado = context.UnidadesResidenciais.Any(u =>
                u.CondominioId == unidade.CondominioId &&
                u.Identificador == unidade.Identificador &&
                u.Id != unidade.Id);

            if (duplicado)
                throw new ArgumentException($"Já existe outra unidade com o identificador '{unidade.Identificador}' neste condomínio.");

            context.UnidadesResidenciais.Update(unidade);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover a unidade residencial da base de dados
        /// </summary>
        /// <param name="id">id da unidade</param>
        public void Delete(int id)
        {
            context.UnidadesResidenciais
                   .Where(u => u.Id == id)
                   .ExecuteDelete();
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
        private static void ValidarUnidade(UnidadesResidenciais unidade)
        {
            if (unidade == null)
                throw new ArgumentException("Unidade residencial inválida.");

            if (string.IsNullOrWhiteSpace(unidade.Identificador))
                throw new ArgumentException("O identificador da unidade é obrigatório.");

            if (unidade.CondominioId <= 0)
                throw new ArgumentException("Condomínio associado inválido.");

            if (!string.IsNullOrWhiteSpace(unidade.Uf) && unidade.Uf.Length != 2)
                throw new ArgumentException("UF deve ter 2 caracteres.");

            if (!string.IsNullOrWhiteSpace(unidade.Cep) && unidade.Cep.Length != 8)
                throw new ArgumentException("CEP deve ter 8 caracteres (somente números).");

            // Telefones são opcionais, mas se informados validamos comprimento mínimo simples
            if (!string.IsNullOrWhiteSpace(unidade.TelefoneResidencial) && unidade.TelefoneResidencial.Length < 8)
                throw new ArgumentException("Telefone residencial inválido.");

            if (!string.IsNullOrWhiteSpace(unidade.TelefoneCelular) && unidade.TelefoneCelular.Length < 8)
                throw new ArgumentException("Telefone celular inválido.");
        }
    }
}