using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace CondosmartWeb.Models
{
    public class VisitanteViewModel
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80, ErrorMessage = "O nome não pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
        [Display(Name = "CPF (Opcional)")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O morador é obrigatório.")]
        [Display(Name = "Morador que está visitando")]
        public int? MoradorId { get; set; }

        [Display(Name = "Observação")]
        [StringLength(200, ErrorMessage = "A observação não pode exceder 200 caracteres.")]
        public string? Observacao { get; set; }

        [Display(Name = "Entrada")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHoraEntrada { get; set; }

        [Display(Name = "Saída")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHoraSaida { get; set; }

        // Lista de moradores para o dropdown
        public List<Morador>? MoradoresDisponiveis { get; set; }
    }
}