using System;
using System.Collections.Generic;

namespace CondosmartWeb.Models;

public partial class Chamado
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime? DataChamado { get; set; }

    public int? MoradorId { get; set; }

    public int? SindicoId { get; set; }

    public int CondominioId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual Moradore? Morador { get; set; }

    public virtual Sindico? Sindico { get; set; }
}
