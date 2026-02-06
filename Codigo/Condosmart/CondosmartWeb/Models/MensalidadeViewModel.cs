using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class MensalidadeViewModel
{
    public int Id { get; set; }

    [Display(Name = "Unidade")]
    [Required(ErrorMessage = "A unidade é obrigatória")]
    public int UnidadeId { get; set; }

    [Display(Name = "Morador")]
    public int? MoradorId { get; set; }

    [Display(Name = "Condomínio")]
    [Required(ErrorMessage = "O condomínio é obrigatório")]
    public int CondominioId { get; set; }

    [Display(Name = "Competência")]
    [Required(ErrorMessage = "A competência é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime Competencia { get; set; }

    [Display(Name = "Vencimento")]
    [Required(ErrorMessage = "O vencimento é obrigatório")]
    [DataType(DataType.Date)]
    public DateTime Vencimento { get; set; }

    [Display(Name = "Valor")]
    [Required(ErrorMessage = "O valor é obrigatório")]
    [DataType(DataType.Currency)]
    [Range(0.01, 999999.99, ErrorMessage = "O valor deve estar entre R$ 0,01 e R$ 999.999,99")]
    public decimal Valor { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "O status é obrigatório")]
    public string Status { get; set; } = null!;

    [Display(Name = "Pagamento")]
    public int? PagamentoId { get; set; }

    [Display(Name = "Data de Criação")]
    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }

    // Propriedades de navegação para exibição
    public string? UnidadeIdentificador { get; set; }
    public string? MoradorNome { get; set; }
    public string? CondominioNome { get; set; }
    
    // Propriedade calculada para verificar se está vencida
    public bool EstaVencida => Status == "pendente" && Vencimento < DateTime.Today;
    
    // Propriedade calculada para verificar se pode ser paga
    public bool PodePagar => (Status == "pendente" || Status == "vencida") && PagamentoId == null;
}
