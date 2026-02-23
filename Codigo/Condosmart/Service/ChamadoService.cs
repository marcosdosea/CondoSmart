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
    /// Implementa serviços para manter dados dos chamados
    /// </summary>
    public class ChamadoService : IChamadosService
    {
        private readonly CondosmartContext context;

        public ChamadoService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo chamado na base de dados
        /// </summary>
        /// <param name="chamado">dados do chamado</param>
        /// <returns>id do chamado</returns>
        public int Create(Chamado chamado)
        {
            // ValidarChamado(chamado); // Removido: validação feita na ViewModel

            try
            {
                context.Add(chamado);
                context.SaveChanges();
                return chamado.Id;
            }
            catch (DbUpdateException ex)
            {
                throw new Core.Exceptions.ServiceException("Erro ao salvar chamado. Verifique se o condomínio existe.", ex);
            }
        }

        /// <summary>
        /// Editar dados do chamado na base de dados
        /// </summary>
        /// <param name="chamado">dados do chamado</param>
        public void Edit(Chamado chamado)
        {
            // ValidarChamado(chamado); // Removido: validação feita na ViewModel

            try
            {
                context.Update(chamado);
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Core.Exceptions.ServiceException("Erro ao atualizar chamado. Verifique se o condomínio existe.", ex);
            }
        }

        /// <summary>
        /// Remover o chamado da base de dados
        /// </summary>
        /// <param name="id">id do chamado</param>
        public void Delete(int id)
        {
            var chamado = context.Chamados.Find(id);
            if (chamado != null)
            {
                context.Remove(chamado);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um chamado na base de dados
        /// </summary>
        /// <param name="id">id do chamado</param>
        /// <returns>dados do chamado ou null</returns>
        public Chamado? GetById(int id)
        {
            return context.Chamados.Find(id);
        }

        /// <summary>
        /// Buscar todos os chamados cadastrados
        /// </summary>
        /// <returns>lista de chamados</returns>
        public List<Chamado> GetAll()
        {
            return context.Chamados.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas do chamado
        /// </summary>
        /// <param name="chamado"></param>
        /*
		private static void ValidarChamado(Chamado chamado)
		{
			if (chamado == null)
				throw new Core.Exceptions.ServiceException("Chamado inválido.");
			// Descrição e CondominioId validados na ViewModel
		}
		*/
    }
}