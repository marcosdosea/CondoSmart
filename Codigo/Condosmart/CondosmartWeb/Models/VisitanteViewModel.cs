using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class VisitanteViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome não pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = null!;

        [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [StringLength(200, ErrorMessage = "A observação não pode exceder 200 caracteres.")]
        [Display(Name = "Observação")]
        public string? Observacao { get; set; }

        [Display(Name = "Data de Entrada")]
        [Required(ErrorMessage = "A data de entrada é obrigatória")]
        public DateTime DataHoraEntrada { get; set; } = DateTime.Now;

        [Display(Name = "Data de Saída")]
        public DateTime? DataHoraSaida { get; set; }
    }
}