using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class PagarMensalidadeViewModel
{
    public int MensalidadeId { get; set; }

    [Display(Name = "Forma de Pagamento")]
    [Required(ErrorMessage = "Selecione a forma de pagamento")]
    [RegularExpression("^(pix|cartao_credito|cartao_debito|boleto|dinheiro)$", ErrorMessage = "Selecione uma forma de pagamento válida.")]
    public string FormaPagamento { get; set; } = null!;

    // Dados da mensalidade para exibição
    public decimal Valor { get; set; }
    public DateTime Competencia { get; set; }
    public DateTime Vencimento { get; set; }
    public string? CondominioNome { get; set; }
    public string? UnidadeIdentificador { get; set; }
    public string? MoradorNome { get; set; }
}
