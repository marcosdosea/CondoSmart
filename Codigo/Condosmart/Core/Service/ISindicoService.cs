using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Service
{
    public interface ISindicoService
    {
        int Create(Sindico sindico);
        void Edit(Sindico sindico);
        void Delete(int id);
        Sindico? GetById(int id);
        List<Sindico> GetAll();
    }
}
