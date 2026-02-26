using System;
using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class LiquidarMensalidadeDTO
    {
        [Required(ErrorMessage = "O ID da mensalidade é obrigatório.")]
        public int MensalidadeId { get; set; }

        [Required(ErrorMessage = "Informe a data efetiva do pagamento.")]
        public DateTime DataPagamento { get; set; }

        [Required(ErrorMessage = "A forma de pagamento é obrigatória.")]
        public string FormaPagamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor pago é obrigatório.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal ValorPago { get; set; }
    }
}