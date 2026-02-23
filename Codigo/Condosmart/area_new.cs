using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Exceptions;

namespace Service
{
    public class AreaDeLazerService : IAreaDeLazerService
    {
        private readonly CondosmartContext context;

        public AreaDeLazerService(CondosmartContext context)
        {
            this.context = context;
        }

        public int Create(AreaDeLazer areaDeLazer)
        {
            context.Add(areaDeLazer);
            context.SaveChanges();
            return areaDeLazer.Id;
        }

        public void Edit(AreaDeLazer areaDeLazer)
        {
            context.Update(areaDeLazer);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var areaDeLazer = context.AreaDeLazer.Find(id);
            if (areaDeLazer != null)
            {
                context.Remove(areaDeLazer);
                context.SaveChanges();
            }
        }

        public AreaDeLazer? GetById(int id)
        {
            return context.AreaDeLazer.Find(id);
        }

        public List<AreaDeLazer> GetAll()
        {
            return context.AreaDeLazer.AsNoTracking().ToList();
        }
    }
}
