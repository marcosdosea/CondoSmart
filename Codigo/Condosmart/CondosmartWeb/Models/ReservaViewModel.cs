using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class ReservaViewModel : IValidatableObject
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Area e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma area valida")]
        [Display(Name = "Area")]
        public int AreaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um morador valido")]
        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um visitante valido")]
        [Display(Name = "Visitante")]
        public int? VisitanteId { get; set; }

        [Required(ErrorMessage = "O campo Condominio e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido")]
        [Display(Name = "Condominio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um sindico valido")]
        [Display(Name = "Sindico")]
        public int? SindicoId { get; set; }

        [Required(ErrorMessage = "A data/hora de inicio e obrigatoria")]
        [Display(Name = "Data de Inicio")]
        public DateTime DataInicio { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A data/hora de fim e obrigatoria")]
        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O status e obrigatorio")]
        [StringLength(20)]
        [RegularExpression("^(pendente|confirmado|cancelado|concluido)$", ErrorMessage = "Selecione um status valido.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "pendente";

        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Area de Lazer")]
        public string? NomeArea { get; set; }

        [Display(Name = "Nome do Morador")]
        public string? NomeMorador { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataFim <= DataInicio)
            {
                yield return new ValidationResult(
                    "A data de fim deve ser posterior a data de inicio.",
                    new[] { nameof(DataFim) });
            }
        }
    }
}
