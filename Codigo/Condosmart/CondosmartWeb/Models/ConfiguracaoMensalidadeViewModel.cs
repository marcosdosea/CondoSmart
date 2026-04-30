using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class ConfiguracaoMensalidadeViewModel
{
    [Required(ErrorMessage = "Selecione um condominio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido.")]
    public int CondominioId { get; set; }

    [Required(ErrorMessage = "Informe o valor da mensalidade.")]
    [Range(0.01, 999999.99, ErrorMessage = "Informe um valor valido para a mensalidade.")]
    [DataType(DataType.Currency)]
    public decimal ValorMensalidade { get; set; }

    [Required(ErrorMessage = "Informe o dia de vencimento.")]
    [Range(1, 28, ErrorMessage = "O dia de vencimento deve ficar entre 1 e 28.")]
    public int DiaVencimento { get; set; } = 10;

    [Required(ErrorMessage = "Informe a quantidade padrao de parcelas.")]
    [Range(1, 12, ErrorMessage = "A quantidade padrao de parcelas deve ficar entre 1 e 12.")]
    public int QuantidadeParcelasPadrao { get; set; } = 12;

    public bool Ativa { get; set; } = true;
}
