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
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        [Display(Name = "Disponibilidade")]
        public bool? Disponibilidade { get; set; } = true;

        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
