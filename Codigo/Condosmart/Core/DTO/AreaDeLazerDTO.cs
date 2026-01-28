using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class AreaDeLazerDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O condomínio é obrigatório.")]
        public int CondominioId { get; set; }

        public bool Disponibilidade { get; set; } = true;
    }
}