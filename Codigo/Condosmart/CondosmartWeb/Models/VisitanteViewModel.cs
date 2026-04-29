using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace CondosmartWeb.Models
{
    public class VisitanteViewModel : IValidatableObject
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80, ErrorMessage = "O nome não pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CPF deve conter somente números (11 dígitos).")]
        [Display(Name = "CPF (Opcional)")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres.")]
        [MinLength(8, ErrorMessage = "Telefone deve ter pelo menos 8 dígitos.")]
        [RegularExpression("^[0-9]{8,11}$", ErrorMessage = "Telefone deve conter somente números (8 a 11 dígitos).")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O morador é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um morador válido")]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataHoraEntrada.HasValue && DataHoraSaida.HasValue && DataHoraSaida <= DataHoraEntrada)
            {
                yield return new ValidationResult(
                    "A data/hora de saída deve ser posterior à entrada.",
                    new[] { nameof(DataHoraSaida) });
            }
        }
    }
}
