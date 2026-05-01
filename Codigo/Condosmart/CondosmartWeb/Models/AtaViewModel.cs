using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CondosmartWeb.Models
{
    public class AtaViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Titulo e obrigatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O titulo deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Temas e obrigatorio")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Os temas devem ter entre 3 e 150 caracteres.")]
        [Display(Name = "Temas")]
        public string Temas { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Data da Reuniao e obrigatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Reuniao")]
        public DateTime DataReuniao { get; set; }

        [Required(ErrorMessage = "O campo Observacoes e obrigatorio")]
        [StringLength(4000, MinimumLength = 10, ErrorMessage = "O conteudo deve ter entre 10 e 4000 caracteres.")]
        [Display(Name = "Observacoes (Markdown)")]
        public string Conteudo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Condominio e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido")]
        [Display(Name = "Condominio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um sindico valido")]
        [Display(Name = "Sindico")]
        public int? SindicoId { get; set; }

        [Display(Name = "Arquivo da Ata")]
        public IFormFile? ArquivoAta { get; set; }

        public string? ArquivoNomeOriginal { get; set; }

        public string? ArquivoCaminho { get; set; }

        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
