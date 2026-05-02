using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CondosmartWeb.Models
{
    public class AreaDeLazerViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome e obrigatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Descricao")]
        [StringLength(200)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo Condominio e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido")]
        [Display(Name = "Condominio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um sindico valido")]
        [Display(Name = "Sindico")]
        public int? SindicoId { get; set; }

        [Required(ErrorMessage = "Informe a disponibilidade")]
        [Display(Name = "Disponibilidade")]
        public bool? Disponibilidade { get; set; } = true;

        [Display(Name = "Imagem da area")]
        public IFormFile? ImagemArquivo { get; set; }

        public string? ImagemNomeOriginal { get; set; }

        public string? ImagemCaminho { get; set; }

        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
