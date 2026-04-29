using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class PagamentoViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Morador")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe um morador válido")]
        public int? MoradorId { get; set; }

        [Display(Name = "Unidade")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe uma unidade válida")]
        public int? UnidadeId { get; set; }

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe um condomínio válido")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Required(ErrorMessage = "O campo Forma de Pagamento é obrigatório")]
        [RegularExpression("^(pix|cartao_credito|cartao_debito|boleto|dinheiro)$", ErrorMessage = "Selecione uma forma de pagamento válida.")]
        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; } = null!;

        [Required(ErrorMessage = "O campo Status é obrigatório")]
        [RegularExpression("^(pendente|pago|cancelado)$", ErrorMessage = "Selecione um status válido.")]
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
