using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace CondosmartWeb.Models
{
    public class VisitanteViewModel : IValidatableObject
    {
        [Key]
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome e obrigatorio.")]
        [StringLength(80, ErrorMessage = "O nome nao pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [StringLength(11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CPF deve conter somente numeros (11 digitos).")]
        [Display(Name = "CPF (Opcional)")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O telefone e obrigatorio.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone deve ter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Telefone deve conter exatamente 11 numeros.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O morador e obrigatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um morador valido")]
        [Display(Name = "Morador que esta visitando")]
        public int? MoradorId { get; set; }

        [Display(Name = "Observacao")]
        [StringLength(200, ErrorMessage = "A observacao nao pode exceder 200 caracteres.")]
        public string? Observacao { get; set; }

        [Display(Name = "Entrada")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHoraEntrada { get; set; }

        [Display(Name = "Saida")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHoraSaida { get; set; }

        public List<Morador>? MoradoresDisponiveis { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataHoraEntrada.HasValue && DataHoraSaida.HasValue && DataHoraSaida <= DataHoraEntrada)
            {
                yield return new ValidationResult(
                    "A data/hora de saida deve ser posterior a entrada.",
                    new[] { nameof(DataHoraSaida) });
            }
        }
    }
}
