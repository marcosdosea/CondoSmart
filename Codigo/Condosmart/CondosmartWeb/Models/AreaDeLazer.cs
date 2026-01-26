using System;
using System.Collections.Generic;

namespace CondosmartWeb.Models;

public partial class AreaDeLazer
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public bool? Disponibilidade { get; set; }

    public string? Descricao { get; set; }

    public int CondominioId { get; set; }

    public int? SindicoId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Sindico? Sindico { get; set; }
}
