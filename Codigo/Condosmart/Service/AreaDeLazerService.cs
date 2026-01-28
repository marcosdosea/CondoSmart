using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class AreaDeLazerService : IAreaDeLazerService
    {
        private readonly CondosmartContext _context;

        public AreaDeLazerService(CondosmartContext context)
        {
            _context = context;
        }

        public int Create(AreaDeLazer area)
        {
            // Validação de Regra de Negócio: Não permite nomes iguais no mesmo condomínio
            if (ExisteNomeNoCondominio(area.Nome, area.CondominioId))
            {
                throw new InvalidOperationException($"Já existe uma área de lazer chamada '{area.Nome}' neste condomínio.");
            }

            // Garante a data de criação
            if (area.CreatedAt == null || area.CreatedAt == DateTime.MinValue)
                area.CreatedAt = DateTime.Now;

            // Define disponibilidade padrão caso venha nula
            if (!area.Disponibilidade.HasValue)
                area.Disponibilidade = true;

            _context.AreaDeLazer.Add(area);
            _context.SaveChanges();
            return area.Id;
        }

        public void Edit(AreaDeLazer area)
        {
            var existing = _context.AreaDeLazer.Find(area.Id);
            if (existing == null) throw new KeyNotFoundException("Área de Lazer não encontrada.");

            // Atualiza apenas os campos permitidos
            existing.Nome = area.Nome;
            existing.Descricao = area.Descricao;
            existing.Disponibilidade = area.Disponibilidade;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.AreaDeLazer.Find(id);
            if (existing == null) throw new KeyNotFoundException("Área de Lazer não encontrada.");

            _context.AreaDeLazer.Remove(existing);
            _context.SaveChanges();
        }

        public AreaDeLazer GetById(int id)
        {
            var area = _context.AreaDeLazer.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (area == null) throw new KeyNotFoundException("Área de Lazer não encontrada.");

            return area;
        }

        public List<AreaDeLazer> GetAll()
        {
            return _context.AreaDeLazer.AsNoTracking().ToList();
        }

        public bool ExisteNomeNoCondominio(string nome, int condominioId)
        {
            // O Trim().ToLower() ajuda a evitar duplicidade tipo "Piscina" e "piscina "
            return _context.AreaDeLazer
                           .Any(a => a.CondominioId == condominioId && a.Nome == nome);
        }
    }
}