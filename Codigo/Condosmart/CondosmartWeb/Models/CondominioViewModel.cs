using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class CondominioViewModel
    {
        [Display(Name = "Código")]
        public int Id {get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome não pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "CNPJ deve ter 14 caracteres.")]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; } = null!;

        [Required(ErrorMessage = "A Rua é obrigatória")]
        [StringLength(80)]
        [Display(Name = "Rua")]
        public string Rua { get; set; } = null!;

        [Required(ErrorMessage = "O Número é obrigatório")]
        [StringLength(10)]
        [Display(Name = "Número")]
        public string Numero { get; set; } = null!;

        [Required(ErrorMessage = "O Bairro é obrigatório")]
        [StringLength(60)]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; } = null!;

        [Required(ErrorMessage = "A Cidade é obrigatória")]
        [StringLength(60)]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = null!;

        [Required(ErrorMessage = "UF é obrigatório")]
        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "UF")]
        public string Uf { get; set; } = null!;

        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8)]
        [Display(Name = "CEP")]
        public string Cep { get; set; } = null!;

        [StringLength(80)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(11)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [StringLength(40)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Unidades é obrigatório")]
        [Display(Name = "Unidades")]
        [Range(1, 100000, ErrorMessage = "Unidades deve ser maior que 0.")]
        public int Unidades { get; set; }
    }
}
