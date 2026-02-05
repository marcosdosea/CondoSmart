using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class PagamentoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [Display(Name = "Descrição (Ex: Condomínio Março)")]
        public string Descricao { get; set; } = null!;

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Display(Name = "Valor (R$)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        [Display(Name = "Vencimento")]
        [DataType(DataType.Date)]
        public DateTime DataVencimento { get; set; }

        [Display(Name = "Data do Pagamento")]
        [DataType(DataType.Date)]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pendente";
    }
}