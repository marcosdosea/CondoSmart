using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Pagamento
{
    public int Id { get; set; }

    public int? MoradorId { get; set; }

    public int? UnidadeId { get; set; }

    public int CondominioId { get; set; }

    public string FormaPagamento { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal Valor { get; set; }

    public DateTime? DataPagamento { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual Morador? Morador { get; set; }

    public virtual UnidadesResidenciais? Unidade { get; set; }
}
