using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class PagamentoViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Display(Name = "Unidade")]
        public int? UnidadeId { get; set; }

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Required(ErrorMessage = "O campo Forma de Pagamento é obrigatório")]
        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; } = null!;

        [Required(ErrorMessage = "O campo Status é obrigatório")]
        [Display(Name = "Status")]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Display(Name = "Valor")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0")]
        public decimal Valor { get; set; }

        [Display(Name = "Data de Pagamento")]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
