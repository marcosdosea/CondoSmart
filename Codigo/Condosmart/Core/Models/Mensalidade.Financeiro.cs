using System;

namespace Core.Models;

public partial class Mensalidade
{
    public decimal ValorOriginal { get; set; }

    public decimal ValorFinal { get; set; }

    public DateTime? DataPagamento { get; set; }
}
