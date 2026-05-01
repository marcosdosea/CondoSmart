using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class ChamadoViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "A descricao e obrigatoria")]
        [StringLength(200, ErrorMessage = "A descricao nao pode exceder 200 caracteres.")]
        [Display(Name = "Descricao")]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Data do Chamado")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "A data do chamado e obrigatoria")]
        public DateTime DataChamado { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Informe um morador valido")]
        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe um sindico valido")]
        [Display(Name = "Sindico")]
        public int? SindicoId { get; set; }

        [Required(ErrorMessage = "O condominio e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe um condominio valido")]
        [Display(Name = "Condominio")]
        public int CondominioId { get; set; }

        [Required(ErrorMessage = "O status e obrigatorio")]
        [StringLength(30, ErrorMessage = "O status nao pode exceder 30 caracteres.")]
        [RegularExpression("^(aberto|em_andamento|resolvido|cancelado)$", ErrorMessage = "Selecione um status valido.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "aberto";

        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
