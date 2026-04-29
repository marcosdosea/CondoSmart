using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class AreaDeLazerViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Descrição")]
        [StringLength(200)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condomínio válido")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um síndico válido")]
        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        [Required(ErrorMessage = "Informe a disponibilidade")]
        [Display(Name = "Disponibilidade")]
        public bool? Disponibilidade { get; set; } = true;

        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
