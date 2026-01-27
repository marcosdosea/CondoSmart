using Core.Models;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IVisitanteService
    {
        int Create(Visitantes visitante);
        void Edit(Visitantes visitante);
        void Delete(int id);
        Visitantes GetById(int id);
        List<Visitantes> GetAll();
    }
}