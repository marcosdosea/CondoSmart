using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Reserva
{
    public int Id { get; set; }

    public int AreaId { get; set; }

    public int? MoradorId { get; set; }

    public int? VisitanteId { get; set; }

    public int CondominioId { get; set; }

    public int? SindicoId { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual AreaDeLazer Area { get; set; } = null!;

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual Morador? Morador { get; set; }

    public virtual Sindico? Sindico { get; set; }

    public virtual Visitantes? Visitante { get; set; }
}
