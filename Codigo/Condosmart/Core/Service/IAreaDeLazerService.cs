using Core.Models;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IAreaDeLazerService
    {
        // Métodos padrões (CRUD)
        int Create(AreaDeLazer area);
        void Edit(AreaDeLazer area);
        void Delete(int id);
        AreaDeLazer GetById(int id);
        List<AreaDeLazer> GetAll();

        // Método específico de Regra de Negócio (para validar duplicidade)
        bool ExisteNomeNoCondominio(string nome, int condominioId);
    }
}