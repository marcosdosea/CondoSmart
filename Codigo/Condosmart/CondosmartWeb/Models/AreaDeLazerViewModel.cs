using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class AreaDeLazerViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O condomínio é obrigatório")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Display(Name = "Disponibilidade")]
        public bool Disponibilidade { get; set; } = true;
    }
}
