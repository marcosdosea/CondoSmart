using System.Collections.Generic;

namespace Core.Models;

public partial class Condominio
{
    public virtual ICollection<ConfiguracaoMensalidade> ConfiguracoesMensalidade { get; set; } = new List<ConfiguracaoMensalidade>();
}
