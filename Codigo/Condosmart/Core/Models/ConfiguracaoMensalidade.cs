using System;

namespace Core.Models;

public partial class ConfiguracaoMensalidade
{
    public int Id { get; set; }

    public int CondominioId { get; set; }

    public decimal ValorMensalidade { get; set; }

    public int DiaVencimento { get; set; }

    public int QuantidadeParcelasPadrao { get; set; }

    public bool Ativa { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;
}
