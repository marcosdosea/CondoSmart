using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class MensalidadeViewModel
{
    public int Id { get; set; }

    [Display(Name = "Unidade")]
    [Required(ErrorMessage = "A unidade e obrigatoria")]
    public int UnidadeId { get; set; }

    [Display(Name = "Morador")]
    public int? MoradorId { get; set; }

    [Display(Name = "Condominio")]
    [Required(ErrorMessage = "O condominio e obrigatorio")]
    public int CondominioId { get; set; }

    [Display(Name = "Competencia")]
    [Required(ErrorMessage = "A competencia e obrigatoria")]
    [DataType(DataType.Date)]
    public DateTime Competencia { get; set; }

    [Display(Name = "Vencimento")]
    [Required(ErrorMessage = "O vencimento e obrigatorio")]
    [DataType(DataType.Date)]
    public DateTime Vencimento { get; set; }

    [Display(Name = "Valor original")]
    [Required(ErrorMessage = "O valor original e obrigatorio")]
    [DataType(DataType.Currency)]
    [Range(0.01, 999999.99, ErrorMessage = "O valor original deve estar entre R$ 0,01 e R$ 999.999,99")]
    public decimal ValorOriginal { get; set; }

    [Display(Name = "Valor final")]
    [Required(ErrorMessage = "O valor final e obrigatorio")]
    [DataType(DataType.Currency)]
    [Range(0.01, 999999.99, ErrorMessage = "O valor final deve estar entre R$ 0,01 e R$ 999.999,99")]
    public decimal ValorFinal { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "O status e obrigatorio")]
    public string Status { get; set; } = string.Empty;

    [Display(Name = "Pagamento")]
    public int? PagamentoId { get; set; }

    [Display(Name = "Data de pagamento")]
    [DataType(DataType.Date)]
    public DateTime? DataPagamento { get; set; }

    [Display(Name = "Data de criacao")]
    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }

    public string? UnidadeIdentificador { get; set; }
    public string? MoradorNome { get; set; }
    public string? CondominioNome { get; set; }

    public bool EstaAtrasada => Status == "atrasado";

    public bool PodePagar => false;
}
