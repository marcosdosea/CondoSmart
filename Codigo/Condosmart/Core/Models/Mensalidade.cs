using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Mensalidade
{
    public int Id { get; set; }

    public int UnidadeId { get; set; }

    public int? MoradorId { get; set; }

    public int CondominioId { get; set; }

    public DateTime Competencia { get; set; }

    public DateTime Vencimento { get; set; }

    public decimal Valor { get; set; }

    public string Status { get; set; } = null!;

    public int? PagamentoId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UnidadesResidenciais Unidade { get; set; } = null!;

    public virtual Morador? Morador { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual Pagamento? Pagamento { get; set; }
}
