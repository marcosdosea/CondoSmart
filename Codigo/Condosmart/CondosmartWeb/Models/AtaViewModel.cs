using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class AtaViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Temas é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Os temas devem ter entre 3 e 150 caracteres.")]
        [Display(Name = "Temas")]
        public string Temas { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Data da Reunião é obrigatório")]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Reunião")]
        public DateTime DataReuniao { get; set; }

        [Required(ErrorMessage = "O campo Conteúdo é obrigatório")]
        [StringLength(4000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 4000 caracteres.")]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condomínio válido")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um síndico válido")]
        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        // Opcional (normalmente não precisa editar, mas pode exibir no Details)
        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
