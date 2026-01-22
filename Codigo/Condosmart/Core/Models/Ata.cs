using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Ata
{
    public int Id { get; set; }

    public int CondominioId { get; set; }

    public int? SindicoId { get; set; }

    public string? Titulo { get; set; }

    public string? Temas { get; set; }

    public string? Conteudo { get; set; }

    public DateOnly? DataReuniao { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual Sindico? Sindico { get; set; }
}
