using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Visitantes // <--- TEM QUE SER "Visitantes" (Plural)
{
    public int Id { get; set; }

    public int? MoradorId { get; set; }

    public int? UnidadeId { get; set; }

    public int? PorteiroId { get; set; }

    public string Nome { get; set; } = null!;

    public string? Cpf { get; set; }

    public string? Telefone { get; set; }

    public string? Observacao { get; set; }

    public DateTime? DataHoraEntrada { get; set; }

    public DateTime? DataHoraSaida { get; set; }

    public int? SindicoId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Morador? Morador { get; set; }

    public virtual Porteiro? Porteiro { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Sindico? Sindico { get; set; }

    public virtual UnidadesResidenciais? Unidade { get; set; }
}