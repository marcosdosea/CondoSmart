using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class UnidadeResidencialViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O número da unidade é obrigatório")]
        [StringLength(10, ErrorMessage = "O número da unidade não pode exceder 10 caracteres.")]
        [Display(Name = "Unidade")]
        public string Numero { get; set; } = null!;

        [Required(ErrorMessage = "O bloco é obrigatório")]
        [StringLength(10, ErrorMessage = "O bloco não pode exceder 10 caracteres.")]
        [Display(Name = "Bloco")]
        public string Bloco { get; set; } = null!;

        [Required(ErrorMessage = "O tipo da unidade é obrigatório")]
        [StringLength(30)]
        [Display(Name = "Tipo da Unidade")]
        public string Tipo { get; set; } = null!;

        [Required(ErrorMessage = "A metragem é obrigatória")]
        [Range(1, 10000, ErrorMessage = "A metragem deve ser maior que 0.")]
        [Display(Name = "Metragem (m²)")]
        public decimal Metragem { get; set; }

        [Required(ErrorMessage = "O número de quartos é obrigatório")]
        [Range(0, 20)]
        [Display(Name = "Quartos")]
        public int Quartos { get; set; }

        [Required(ErrorMessage = "O número de banheiros é obrigatório")]
        [Range(0, 20)]
        [Display(Name = "Banheiros")]
        public int Banheiros { get; set; }

        [Display(Name = "Vagas de Garagem")]
        [Range(0, 20)]
        public int VagasGaragem { get; set; }

        [Required(ErrorMessage = "Informe o condomínio")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }
    }
}
