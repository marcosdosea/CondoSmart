using Core.Models;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IVisitanteService
    {
        int Create(Visitantes visitante);

        void Edit(Visitantes visitante);

        void Delete(int id);

        Visitantes? Get(int id);

        IEnumerable<Visitantes> GetAll();

        IEnumerable<Visitantes> GetByNome(string nome);
    }
}