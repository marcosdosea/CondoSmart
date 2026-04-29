using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class ReservaViewModel : IValidatableObject
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Área é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma área válida")]
        [Display(Name = "Área")]
        public int AreaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um morador válido")]
        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um visitante válido")]
        [Display(Name = "Visitante")]
        public int? VisitanteId { get; set; }

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condomínio válido")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um síndico válido")]
        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        [Required(ErrorMessage = "A data/hora de início é obrigatória")]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A data/hora de fim é obrigatória")]
        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O status é obrigatório")]
        [StringLength(20)]
        [RegularExpression("^(pendente|confirmado|cancelado|concluido)$", ErrorMessage = "Selecione um status válido.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "pendente";

        // Opcional: exibir quando foi criado
        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Área de Lazer")]
        public string? NomeArea {get; set; }

        [Display(Name = "Nome do Morador")]
        public string? NomeMorador { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataFim <= DataInicio)
            {
                yield return new ValidationResult(
                    "A data de fim deve ser posterior à data de início.",
                    new[] { nameof(DataFim) });
            }
        }
    }
}
