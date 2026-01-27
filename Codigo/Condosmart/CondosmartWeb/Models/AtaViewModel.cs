using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class AtaViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título não pode exceder 100 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Temas é obrigatório")]
        [StringLength(150, ErrorMessage = "Os temas não podem exceder 150 caracteres.")]
        [Display(Name = "Temas")]
        public string Temas { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Data da Reunião é obrigatório")]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Reunião")]
        public DateTime DataReuniao { get; set; }

        [Required(ErrorMessage = "O campo Conteúdo é obrigatório")]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        // Opcional (normalmente não precisa editar, mas pode exibir no Details)
        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}
