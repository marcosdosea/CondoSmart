using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class ComprovantePagamentoViewModel
{
    // Dados da Mensalidade
    public int MensalidadeId { get; set; }
    public DateTime Competencia { get; set; }
    public DateTime Vencimento { get; set; }
    public decimal ValorMensalidade { get; set; }
    public string? CondominioNome { get; set; }
    public string? UnidadeIdentificador { get; set; }
    public string? MoradorNome { get; set; }

    // Dados do Pagamento
    public int PagamentoId { get; set; }
    
    [Display(Name = "Forma de Pagamento")]
    public string FormaPagamento { get; set; } = null!;
    
    [Display(Name = "Status do Pagamento")]
    public string StatusPagamento { get; set; } = null!;
    
    [Display(Name = "Valor Pago")]
    public decimal ValorPago { get; set; }
    
    [Display(Name = "Data do Pagamento")]
    public DateTime? DataPagamento { get; set; }
    
    [Display(Name = "Data de Registro")]
    public DateTime? DataRegistro { get; set; }

    public string FormaPagamentoFormatada
    {
        get
        {
            return FormaPagamento switch
            {
                "pix" => "PIX",
                "cartao_credito" => "Cartão de Crédito",
                "cartao_debito" => "Cartão de Débito",
                "boleto" => "Boleto Bancário",
                "dinheiro" => "Dinheiro",
                _ => FormaPagamento
            };
        }
    }
}
