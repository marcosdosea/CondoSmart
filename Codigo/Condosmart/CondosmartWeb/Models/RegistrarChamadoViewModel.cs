using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels
{
    public class RegistrarChamadoViewModel
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 500 caracteres.")]
        [Display(Name = "Descreva o problema")]
        public string Descricao { get; set; } = string.Empty;
    }
}